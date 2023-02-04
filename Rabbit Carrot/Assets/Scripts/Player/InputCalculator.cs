using Mono.Cecil.Pdb;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputCalculator
{
    /// <summary>
    /// History recording frames length.
    /// </summary>
    private const int LISTEN_FRAMES = 60;

    private KeyInputBuffer buffer = new KeyInputBuffer(LISTEN_FRAMES);
    private Coroutine refreshCoroutine;

    private E_PlayerOperation nowClimbOperation; //now operations of climbing

    #region ListeningKeys
    private InputSource climbUpKey = (KeyCodeSource)KeyCode.None;
    /// <summary>
    /// The key used to climb up.
    /// </summary>
    public InputSource ClimbUpKey 
    {
        get => climbUpKey;
        set
        {
            buffer.ChangeKey(climbUpKey, value);
            climbUpKey = value;
        }
    }

    private InputSource climbDownKey = (KeyCodeSource)KeyCode.None;
    /// <summary>
    /// The key used to climb down.
    /// </summary>
    public InputSource ClimbDownKey
    {
        get => climbDownKey;
        set
        {
            buffer.ChangeKey(climbDownKey, value);
            climbDownKey = value;
        }
    }

    private InputSource moveLeftKey = (KeyCodeSource)KeyCode.None;
    /// <summary>
    /// The key used to move mole left.
    /// </summary>
    public InputSource MoveLeftKey
    {
        get => moveLeftKey;
        set
        {
            buffer.ChangeKey(moveLeftKey, value);
            moveLeftKey = value;
        }
    }

    private InputSource moveRightKey = (KeyCodeSource)KeyCode.None;
    /// <summary>
    /// The key used to move mole left.
    /// </summary>
    public InputSource MoveRightKey
    {
        get => moveRightKey;
        set
        {
            buffer.ChangeKey(moveRightKey, value);
            moveRightKey = value;
        }
    }
    #endregion

    /// <summary>
    /// Whether the calculator is processing and recoding the player inputs.
    /// </summary>
    public bool IsOperating { get; private set; }
    public E_PlayerOperation[] GetOperation()
    {
        ///How many continously times the input have triggered. 
        ///For keyboard keys, it returns how many time the key was pressed during the history.
        int InputTriggeredTime(bool[] history) 
        {
            int time = 0;
            if(history.Length > 0 && history[0] == true)
            {
                time++;
            }
            for(int i = 1; i < history.Length; i++)
            {
                if (history[i - 1] == false && history[i] == true)
                    time++;
            }
            return time;
        }

        List<E_PlayerOperation> operations= new List<E_PlayerOperation>();

        bool[] upHistory = buffer.GetHistory(ClimbUpKey);
        int upTimes = InputTriggeredTime(upHistory); //how many times climb up key has been pressed during the history
        bool[] downHistory = buffer.GetHistory(ClimbDownKey);
        int downTimes = InputTriggeredTime(downHistory);//how many times climb down key has been pressed during the history
        
        if (upHistory[upHistory.Length - 1] == true && downHistory[downHistory.Length - 1] == true)
        {
            //When both key triggered, clear all history 
            buffer.ClearHistory(ClimbUpKey);
            buffer.ClearHistory(ClimbDownKey);
            operations.Add(E_PlayerOperation.None);
            nowClimbOperation = E_PlayerOperation.None;
        }
        else
        {
            switch (nowClimbOperation)
            {
                case E_PlayerOperation.None:
                    { 
                        if (upHistory[upHistory.Length - 1] == true)
                        {
                            if (upTimes == 1)
                            {
                                operations.Add(E_PlayerOperation.ClimbUp);
                                nowClimbOperation = E_PlayerOperation.ClimbUp;
                            }
                            else if (upTimes > 1)
                            {
                                operations.Add(E_PlayerOperation.ClimbUpQuick);
                                nowClimbOperation = E_PlayerOperation.ClimbUpQuick;
                            }
                        }
                        if (downHistory[downHistory.Length - 1] == true)
                        {
                            if (downTimes == 1)
                            {
                                operations.Add(E_PlayerOperation.ClimbDown);
                                nowClimbOperation = E_PlayerOperation.ClimbDown;
                            }
                            else if (downTimes > 1)
                            {
                                operations.Add(E_PlayerOperation.ClimbDownQuick);
                                nowClimbOperation = E_PlayerOperation.ClimbDownQuick;
                            }
                        }
                    }
                    break;
                case E_PlayerOperation.ClimbUp:
                case E_PlayerOperation.ClimbUpQuick:
                    {
                        if (upHistory[upHistory.Length - 1] == false)
                        {
                            nowClimbOperation = E_PlayerOperation.None;
                        }
                        else
                        {
                            operations.Add(nowClimbOperation); //Stay at present operation.
                        }
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                case E_PlayerOperation.ClimbDownQuick:
                    {
                        if (downHistory[downHistory.Length - 1] == false)
                        {
                            nowClimbOperation = E_PlayerOperation.None;
                        }
                        else
                        {
                            operations.Add(nowClimbOperation); //Stay at present operation.
                        }
                    }
                    break;
            }
        }
        

        //For moving mole operations was simple.
        bool[] leftHistory = buffer.GetHistory(MoveLeftKey);
        bool[] rightHistory = buffer.GetHistory(MoveRightKey);
        bool isLeft = leftHistory[leftHistory.Length - 1];
        bool isRight = rightHistory[rightHistory.Length - 1];
        if (isLeft && !isRight) 
        {
            operations.Add(E_PlayerOperation.MoleMoveLeft);
        }
        else if(!isLeft && isRight)
        {
            operations.Add(E_PlayerOperation.MoleMoveRight);
        }

        return operations.ToArray();
    }
    private IEnumerator KeyRefreshment()
    {
        while (GameController.Instance.IsPlaying && IsOperating)
        {
            foreach (var source in buffer.ListeningSources)
            {
                buffer.RefreshKey(source, source.GetInputStatus());

                //Code for debuging: print the history of input source.
                //string str = source.ToString() + ":";
                //bool print = false;
                //foreach (bool item in buffer.GetHistory(source))
                //{
                //    str += item ? 1 : 0;
                //    if (item)
                //        print = true;
                //}
                //if (print)
                //    Debug.Log(str);
            }
            yield return 1;
        }
        IsOperating = false;
    }
    /// <summary>
    /// Start listening to player input.
    /// </summary>
    public void StartListening()
    {
        IsOperating = true;
        refreshCoroutine = MonoManager.Instance.StartCoroutine(KeyRefreshment());
    }
    /// <summary>
    /// Stop listening to player input.
    /// </summary>
    public void StopListening()
    {
        IsOperating = false;
        MonoManager.Instance.StopCoroutine(refreshCoroutine);
    }
}
/// <summary>
/// A buffer to save the previous key kode pressed by player.
/// </summary>
public class KeyInputBuffer
{
    private int listenFrames;
    private Dictionary<InputSource, Queue<bool>> keyDic = new Dictionary<InputSource, Queue<bool>>();

    public ICollection<InputSource> ListeningSources => keyDic.Keys;
    /// <summary>
    /// Create a buffer with total frames the buffer will be listening for the player input.
    /// </summary>
    /// <param name="listeningTotalFrames">The total frames</param>
    public KeyInputBuffer(int listeningTotalFrames)
    {
        listenFrames = listeningTotalFrames;
    }

    public void AddListening(InputSource source)
    {
        if (!keyDic.ContainsKey(source))
        {
            keyDic.Add(source, new Queue<bool>(listenFrames));
        }
    }
    public void RemoveListening(InputSource source)
    {
        if (keyDic.ContainsKey(source))
        {
            keyDic.Remove(source);
        }
    }
    /// <summary>
    /// Change a key listening along with its history to the new key.
    /// The previous key must be listening and the next key must not be listening.
    /// </summary>
    /// <param name="previous"></param>
    /// <param name="next"></param>
    public void ChangeKey(InputSource previous, InputSource next)
    {
        if (!keyDic.ContainsKey(previous))
        {
            keyDic.Add(previous, new Queue<bool>());
        }
        if (!keyDic.ContainsKey(next))
        {
            Queue<bool> queue = keyDic[previous];
            keyDic.Remove(previous);
            keyDic.Add(next, queue);
        }
    }
    /// <summary>
    /// Refresh the key status.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="isPressed"></param>
    public void RefreshKey(InputSource source, bool isPressed)
    {
        if (keyDic.ContainsKey(source))
        {
            Queue<bool> queue = keyDic[source];
            if (queue.Count == listenFrames)
            {
                queue.Dequeue();
            }
            queue.Enqueue(isPressed);
        }
    }
    /// <summary>
    /// Get key input history in buffer.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool[] GetHistory(InputSource source)
    {
        if (keyDic.ContainsKey(source))
        {
            if (keyDic[source].Count > 0)
                return keyDic[source].ToArray();
            else
                return new bool[1];
        }
        else
            return null;
    }
    /// <summary>
    /// Remove all history of input source.
    /// </summary>
    /// <param name="source"></param>
    public void ClearHistory(InputSource source)
    {
        if(keyDic.ContainsKey(source))
            keyDic[source].Clear();
    }
}
/// <summary>
/// A input source includes key and mouse.
/// </summary>
public interface InputSource
{
    public bool GetInputStatus();
}
public class KeyCodeSource : InputSource
{
    private KeyCode key;
    
    public KeyCodeSource(KeyCode key)
    {
        this.key = key;
    }

    public bool GetInputStatus()
    {
        return Input.GetKey(key);
    }
    public override string ToString()
    {
        return key.ToString();
    }
    public static explicit operator KeyCodeSource(KeyCode key)
    {
        return new KeyCodeSource(key);
    }
}
public class MouseSource : InputSource
{
    private int mouseBtn;
    public MouseSource(int mouseBtn)
    {
        this.mouseBtn = mouseBtn;
    }

    public bool GetInputStatus()
    {

        return Input.GetMouseButton(mouseBtn) && !EventSystem.current.IsPointerOverGameObject();
    }
    public override string ToString()
    {
        return "MouseButton:" + mouseBtn;
    }
    public static explicit operator MouseSource(int mouseBtn)
    {
        return new MouseSource(mouseBtn);
    }
}