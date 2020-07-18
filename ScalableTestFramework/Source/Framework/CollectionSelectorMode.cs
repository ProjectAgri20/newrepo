namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Methods that can be used for selecting elements from a collection.
    /// </summary>
    public enum CollectionSelectorMode
    {
        /// <summary>
        /// Elements are returned in the order in which they appear in the collection.
        /// </summary>
        RoundRobin,

        /// <summary>
        /// Elements are assigned a random order and then returned in that order.
        /// A new random order is applied each time the list is enumerated.
        /// </summary>
        /// <remarks>
        /// This selection method does not modify the order of elements in the collection.
        /// Only the order of selection is shuffled.
        /// </remarks>
        ShuffledRoundRobin,

        /// <summary>
        /// Elements are selected from the collection at random.
        /// </summary>
        /// <remarks>
        /// This method does not guarantee an equal distribution of elements.  Some elements might
        /// be selected multiple times even though others have not been selected yet.
        /// </remarks>
        Random
    }
}
