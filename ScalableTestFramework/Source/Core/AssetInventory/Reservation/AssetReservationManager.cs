using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Reservation
{
    /// <summary>
    /// Handles <see cref="AssetReservation" /> creation and management.
    /// </summary>
    public sealed class AssetReservationManager
    {
        private const string _duplicateReservationConstraint = "CK_DuplicateReservations";

        private readonly AssetInventoryConnectionString _connectionString;
        private readonly string _requester;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetReservationManager" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <param name="requester">The source that is requesting the reservation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public AssetReservationManager(AssetInventoryConnectionString connectionString, string requester)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _requester = requester;
        }

        /// <summary>
        /// Creates asset reservations for the specified time period.
        /// Attempts to reserve devices for the entire period, but will create a partial reservation
        /// if the device is not available for the entire time.
        /// </summary>
        /// <param name="assetIds">The assets to reserve.</param>
        /// <param name="sessionId">The session ID to reserve for.</param>
        /// <param name="reservationKey">The reservation key indicating which existing reservations may be used.</param>
        /// <param name="requestedStart">The requested start.</param>
        /// <param name="requestedEnd">The requested end.</param>
        /// <returns>A collection of <see cref="AssetReservationResult" /> describing the reservations that could be made.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetIds" /> is null.</exception>
        public IEnumerable<AssetReservationResult> ReserveAssets(IEnumerable<string> assetIds, string sessionId, string reservationKey, DateTime requestedStart, DateTime requestedEnd)
        {
            if (assetIds == null)
            {
                throw new ArgumentNullException(nameof(assetIds));
            }

            List<AssetReservationResult> results = new List<AssetReservationResult>();

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                //Include associated Assets, like Cameras
                List<string> assetIdsFinal = assetIds.ToList();
                assetIdsFinal.AddRange(GetAssociatedCameras(context, assetIds));

                LogInfo($"Reserving {assetIdsFinal.Count()} assets for session {sessionId}.");

                // Get all the data from the database at once, but then loop through the requested IDs to see if there are any missing
                List<Asset> assets = context.Assets.Include(n => n.Pool).Include(n => n.Reservations).Where(n => assetIdsFinal.Contains(n.AssetId)).ToList();
                foreach (string assetId in assetIdsFinal)
                {
                    Asset asset = assets.FirstOrDefault(n => n.AssetId == assetId);
                    if (asset != null)
                    {
                        CreateReservations(asset, sessionId, reservationKey, requestedStart, requestedEnd);
                        AssetReservationResult result = GetReservationResult(asset, sessionId, requestedStart, requestedEnd);
                        results.Add(result);
                    }
                    else
                    {
                        // Asset was not found in database
                        LogWarn($"Asset {assetId} was not found in Asset Inventory database.");
                        results.Add(new AssetReservationResult(assetId, AssetAvailability.Unknown));
                    }
                }

                // Update ReservationHistory where configured to do so
                foreach (Asset asset in assets.Where(n => n.Pool.TrackReservations == true))
                {
                    foreach (AssetReservation reservation in asset.Reservations.Where(n => context.Entry(n).State == EntityState.Added))
                    {
                        context.ReservationHistory.Add(new ReservationHistory(reservation));
                    }
                }

                // Commit the reservations to the database
                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateException ex) when (ex.InnerException?.InnerException?.Message?.Contains(_duplicateReservationConstraint) == true)
                {
                    // This may happen due to concurrency conflicts, e.g. somebody else creates a reservation
                    // after we have retrieved the existing reservations from the database but before we attempt to save them back.
                    throw new ResourceReservationException("Failed to create reservation due to a conflict with an existing reservation.", ex);
                }
            }

            return results;
        }

        /// <summary>
        /// Getting a list of cameras ids associated with the printers.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetIds"></param>
        /// <returns></returns>
        private static List<string> GetAssociatedCameras(AssetInventoryContext context, IEnumerable<string> assetIds)
        {
            return context.Assets.OfType<Camera>().Where(c => assetIds.Contains(c.PrinterId)).Select(a => a.AssetId).ToList();
        }

        private void CreateReservations(Asset asset, string sessionId, string reservationKey, DateTime requestedStart, DateTime requestedEnd)
        {
            // Find any reservations that overlap with the requested time
            List<AssetReservation> overlappingReservations = asset.Reservations.Where(n => n.End > requestedStart && n.Start < requestedEnd).OrderBy(n => n.Start).ToList();

            // Track two items: first, whether we have started creating a reservation so far;
            // second, the time from which we are trying to create/extend the reservation period
            bool obtainedAnyReservation = false;
            DateTime currentTime = requestedStart;

            // Walk through the requested time period, using existing reservations where possible and creating new reservations in the blank spaces
            for (int i = 0; i < overlappingReservations.Count; i++)
            {
                AssetReservation nextExistingReservation = overlappingReservations[i];

                // Check to see if there is a blank space before the next existing reservation - if so, fill it in
                if (nextExistingReservation.Start > currentTime.AddMilliseconds(10))
                {
                    CreateReservation(asset, sessionId, currentTime, nextExistingReservation.Start);
                    obtainedAnyReservation = true;
                }

                // Check to see if we can use the existing reservation for this session
                if (nextExistingReservation.ReservedFor.EqualsIgnoreCase(reservationKey) && string.IsNullOrEmpty(nextExistingReservation.SessionId))
                {
                    nextExistingReservation.SessionId = sessionId;
                    currentTime = nextExistingReservation.End;
                    obtainedAnyReservation = true;
                }
                else if (!obtainedAnyReservation)
                {
                    // Still haven't found a block to reserve, so reset the cursor to after this reservation and keep looking
                    currentTime = nextExistingReservation.End;
                }
                else
                {
                    // We have created a partial reservation, but have now reached a point where we cannot extend it further
                    return;
                }
            }

            // Reserve the last block after the existing reservation, if needed.
            // (If there were no other reservations to begin with, this will reserve the entire requested time.)
            if (requestedEnd > currentTime.AddMilliseconds(10))
            {
                CreateReservation(asset, sessionId, currentTime, requestedEnd);
            }
        }

        private AssetReservation CreateReservation(Asset asset, string sessionId, DateTime requestedStart, DateTime requestedEnd)
        {
            // If we're at the end of the day, kick the start time over to the next day
            if (requestedStart.TimeOfDay > new TimeSpan(0, 23, 59, 59, 995))
            {
                requestedStart = requestedStart.AddMilliseconds(10).Date;
            }

            AssetReservation newReservation = new AssetReservation
            {
                AssetReservationId = SequentialGuid.NewGuid(),
                ReservedBy = _requester,
                ReservedFor = sessionId,
                Start = requestedStart,
                End = requestedEnd,
                Received = DateTime.Now,
                SessionId = sessionId,
                CreatedBy = _requester,
                Notify = AssetReservationExpirationNotify.DoNotNotify
            };

            asset.Reservations.Add(newReservation);
            return newReservation;
        }

        private static AssetReservationResult GetReservationResult(Asset asset, string sessionId, DateTime requestedStart, DateTime requestedEnd)
        {
            // Obtain a list of the reservations for this session - the algorithm above ensures they are contiguous
            List<AssetReservation> sessionReservations = asset.Reservations.Where(n => n.SessionId == sessionId).ToList();

            if (sessionReservations.Any())
            {
                DateTime availabilityStart = sessionReservations.Min(n => n.Start);
                DateTime availabilityEnd = sessionReservations.Max(n => n.End);
                if (availabilityStart.AddMilliseconds(-10) <= requestedStart && availabilityEnd.AddMilliseconds(10) >= requestedEnd)
                {
                    return new AssetReservationResult(asset.AssetId, AssetAvailability.Available, availabilityStart, availabilityEnd);
                }
                else
                {
                    return new AssetReservationResult(asset.AssetId, AssetAvailability.PartiallyAvailable, availabilityStart, availabilityEnd);
                }
            }
            else
            {
                return new AssetReservationResult(asset.AssetId, AssetAvailability.NotAvailable);
            }
        }

        /// <summary>
        /// Releases all reservations for the specified session.
        /// </summary>
        /// <param name="sessionId">The session ID to release.</param>
        public void ReleaseSessionReservations(string sessionId)
        {
            ReleaseSessionReservations(new[] { sessionId });
        }

        /// <summary>
        /// Releases all reservations for the specified sessions.
        /// </summary>
        /// <param name="sessionIds">The session IDs to release.</param>
        public void ReleaseSessionReservations(IEnumerable<string> sessionIds)
        {
            LogInfo($"Releasing asset reservations for session(s): {string.Join(", ", sessionIds)}");

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                foreach (AssetReservation reservation in context.AssetReservations.Where(n => sessionIds.Contains(n.SessionId)))
                {
                    if (reservation.ReservedFor == reservation.SessionId)
                    {
                        // This reservation was added by the STF console and should be deleted
                        UpdateReservationHistory(context, reservation);
                        context.AssetReservations.Remove(reservation);
                    }
                    else
                    {
                        // This reservation was made before the session was run - just clear the session ID
                        reservation.SessionId = null;
                    }
                }

                context.SaveChanges();
            }
        }

        private static void UpdateReservationHistory(AssetInventoryContext context, AssetReservation reservation)
        {
            ReservationHistory history = context.ReservationHistory
                                                .OrderByDescending(n => n.ReservationHistoryId)
                                                .FirstOrDefault(n => n.AssetId == reservation.AssetId && n.Start == reservation.Start);

            if (history != null && DateTime.Now < history.End)
            {
                history.End = DateTime.Now;
            }
        }
    }
}
