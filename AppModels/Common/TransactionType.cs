namespace AppModels.Common
{
    /// <summary>
    /// Defines the type of transaction (Income or Outcome)
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Material coming into the store
        /// </summary>
        Income = 1,

        /// <summary>
        /// Material going out of the store
        /// </summary>
        Outcome = 2
    }
}
