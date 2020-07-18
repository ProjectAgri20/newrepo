using System;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Provides methods for submitting and updating <see cref="ActivityDataLog" /> records.
    /// </summary>
    public interface IDataLogger
    {
        /// <summary>
        /// Submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        bool Submit(ActivityDataLog dataLog);

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if update was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        bool Update(ActivityDataLog dataLog);

        /// <summary>
        /// Asynchronously submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        void SubmitAsync(ActivityDataLog dataLog);

        /// <summary>
        /// Asynchronously updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        void UpdateAsync(ActivityDataLog dataLog);
    }
}
