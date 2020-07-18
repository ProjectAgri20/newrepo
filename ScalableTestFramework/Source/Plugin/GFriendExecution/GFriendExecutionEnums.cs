using System.ComponentModel;

namespace HP.ScalableTest.Plugin.GFriendExecution
{
    /// <summary>
    /// Enums for GFriend file types
    /// </summary>
    public enum GFFileTypes
    {

        /// <summary>
        /// Unknown file type. 
        /// This enum value can be used without error if GFriend is updated with other file type.
        /// In common case, this file type shall not be used.
        /// </summary>
        [Description("Unkown File type")]
        Unknown,

        /// <summary>
        /// GFriend Script which contains GFriend test case (*.txt of *.gfscript)
        /// </summary>
        [Description("GFriend Script File")]
        GFScript,

        /// <summary>
        /// GFriend User Variables (*.gfvar)
        /// </summary>
        [Description("GFriend User Varialbes")]
        GFVariable,

        /// <summary>
        /// GFriend User Custom Library (*.gflib)
        /// </summary>
        [Description("GFriend Custom Library")]
        GFLibrary
    }
}
