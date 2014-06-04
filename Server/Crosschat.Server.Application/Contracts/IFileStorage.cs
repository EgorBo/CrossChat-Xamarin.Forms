namespace Crosschat.Server.Application.Contracts
{
    public interface IFileStorage
    {
        /// <summary>
        /// Append new file data and receive it's id
        /// </summary>
        int AppendFile(byte[] file);

        /// <summary>
        /// Runs defragmentation over stored files, this operation is time consuming
        /// </summary>
        void ShrinkStorage(int[] actualIds);
    }
}
