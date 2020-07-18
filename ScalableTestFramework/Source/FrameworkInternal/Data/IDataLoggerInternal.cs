using System;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Internal extension to <see cref="IDataLogger" /> that supports <see cref="FrameworkDataLog" /> records.
    /// </summary>
    public interface IDataLoggerInternal : IDataLogger
    {
        /// <summary>
        /// Submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        bool Submit(FrameworkDataLog dataLog);

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if update was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        bool Update(FrameworkDataLog dataLog);

        /// <summary>
        /// Asynchronously submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        void SubmitAsync(FrameworkDataLog dataLog);

        /// <summary>
        /// Asynchronously updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        void UpdateAsync(FrameworkDataLog dataLog);
    }
}
