namespace Structurizer
{
    /// <summary>
    /// Builds <see cref="IStructure"/> instances from sent Items.
    /// </summary>
    public interface IStructureBuilder
    {
        /// <summary>
        /// Creates a single <see cref="IStructure"/> for sent <typeparamref name="T"/> item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        IStructure CreateStructure<T>(T item) where T : class;

        /// <summary>
        /// Creates one <see cref="IStructure"/> for each sent item in <paramref name="items"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        IStructure[] CreateStructures<T>(T[] items) where T : class;
    }
}