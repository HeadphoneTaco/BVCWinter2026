using UnityEngine;

namespace Interfaces
{
    /// <summary>
    /// Defines behavior for objects that can be collected by another game object.
    /// </summary>
    public interface ICollectable
    {
        /// <summary>
        /// Handles the collection event when this object is collected.
        /// </summary>
        /// <param name="collector">The game object that collected this collectable.</param>
        void OnCollect(GameObject collector);
    }
}