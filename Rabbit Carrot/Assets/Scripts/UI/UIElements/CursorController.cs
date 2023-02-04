using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController
{
    private Texture2D normal;
    private Texture2D targeting;
    private Status status;

    /// <summary>
    /// Create a cursor controller.
    /// </summary>
    /// <param name="normal"></param>
    /// <param name="targeting"></param>
    public CursorController(Texture2D normal, Texture2D targeting)
    {
        this.normal = normal;
        this.targeting = targeting;
        CursorStatus = Status.None;
    }

    public Status CursorStatus
    {
        get { return status; }
        set
        {
            switch (value)
            {
                case Status.None:
                    Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware);
                    break;
                case Status.Targeting:
                    Cursor.SetCursor(targeting, targeting.texelSize / 2, CursorMode.ForceSoftware);
                    break;
            }
            status = value;
        }
    }
    public enum Status
    {
        /// <summary>
        /// Cursor in normol mode
        /// </summary>
        None,
        /// <summary>
        /// Cursor in targeting mode.
        /// </summary>
        Targeting,
    }
}
