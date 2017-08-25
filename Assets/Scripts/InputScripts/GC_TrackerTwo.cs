using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LibUsbDotNet;
using LibUsbDotNet.Main;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

using UnityEngine;




public class GC_TrackerTwo : DeviceTracker
{
    Thread gcThread;

    public UsbEndpointReader reader = null;
    public UsbEndpointWriter writer = null;
    public UsbTransfer usbReaderTransfer = null;
    public UsbDevice GCNAdapter = null;

    byte[] ReadBuffer;
    //byte[] WriteBuffer;
    int transferLength;

    bool keepReading = true;

    float calibrModAnalogYPlus, calibrModAnalogYMinus, calibrModAnalogXPlus, calibrModAnalogXMinus;
    float calibrModcstickYPlus, calibrModcstickYMinus, calibrModcstickXPlus, calibrModcstickXMinus;
    //bool logBoolean;
    bool isModCStick;

    void Awake()
    {

        base.Awake();

        isModCStick = false;
        calibrModAnalogYPlus = calibrModAnalogYMinus = calibrModAnalogXPlus = calibrModAnalogXMinus = 1f;
        calibrModcstickYPlus = calibrModcstickYMinus = calibrModcstickXPlus = calibrModcstickXMinus = 1f;

        

    }

    private void Start()
    {
        var USBFinder = new UsbDeviceFinder(0x057E, 0x0337);
        GCNAdapter = UsbDevice.OpenUsbDevice(USBFinder);


        if (GCNAdapter != null)
        {



            reader = GCNAdapter.OpenEndpointReader(ReadEndpointID.Ep01);
            writer = GCNAdapter.OpenEndpointWriter(WriteEndpointID.Ep02);

            //prompt controller to start sending
            writer.Write(Convert.ToByte((char)19), 10, out transferLength);

            // PORT 1: bytes 02-09
            // PORT 2: bytes 11-17
            // PORT 3: bytes 20-27
            // PORT 4: bytes 29-36l
            ReadBuffer = new byte[37]; // 32 (4 players x 8) bytes for input, 5 bytes for formatting
            //WriteBuffer = new byte[5]; // 1 for command, 4 for rumble state
            //WriteBuffer[0] = 0x11;
            //WriteBuffer[1] = 0;
            reader.ReadThreadPriority = System.Threading.ThreadPriority.Highest;
            reader.Flush();
            gcThread = new Thread(() => inputUpdate(ref ReadBuffer));
            gcThread.Start();
        }
        else
        {
            UnityEngine.Debug.Log("ERROR: Could not detect device WUP-028 Gamecube Controller Adapter.");

        }
    }

    private void inputUpdate(ref byte[] e)
    {
        while(keepReading)
        {
            reader.SubmitAsyncTransfer(e, 0, e.Length, 8, out usbReaderTransfer);
            usbReaderTransfer.Wait(out transferLength);
        }       
    }

    void FixedUpdate()
    {

        readerReadData(ref ReadBuffer);

        setCalibrationModifiers(); //Change this later to using only cstick and analog with start and Z as the trigger+
        applyDeadzone();

        #region Debug
        //if (Time.frameCount % 2 == 0)
        //{
            //UnityEngine.Debug.Log("analogX&Y: (" + data.axes[1] + ", " + data.axes[0] + ").");
            //UnityEngine.Debug.Log("ABXY: (" + data.buttons[0] + ", " + data.buttons[1] + ", " + data.buttons[2] + ", " + data.buttons[3] + ").");
            //UnityEngine.Debug.Log("Start: (" +  data.buttons[7] + ").");
            //UnityEngine.Debug.Log("UDLR: (" + data.buttons[8] + ", " + data.buttons[9] + ", " + data.buttons[10] + ", " + data.buttons[11] + ").");
            //UnityEngine.Debug.Log("Analog Calibration Modifiers: (" + calibrModAnalogYPlus + ", " + calibrModAnalogYMinus + ", " + calibrModAnalogXMinus + ", " + calibrModAnalogXPlus + ").");
            //logBoolean = false;
        //}
        #endregion

        im.PassInput(ref data);//send data
        reader.Dispose();
        data.Reset();
    }

    private void LateUpdate()
    {
    }

    public override void Refresh()
    {
        im = GetComponent<InputManager>();
        //Create 2 temp arrays for buttons and axes
        KeyCode[] newButtons = new KeyCode[im.buttonCount];
        AxisKeys[] newAxes = new AxisKeys[im.axisCount];

    }

    private void setCalibrationModifiers()
    {
        #region Analog Calibration
        if (!isModCStick && data.buttons[7] && data.buttons[4])
        {
            isModCStick = true;
        }

        if (!isModCStick && data.buttons[8] && data.buttons[7])
        {
            calibrModAnalogYPlus = (1f / data.axes[0]);
        }

        if (!isModCStick && data.buttons[9] && data.buttons[7])
        {
            calibrModAnalogYMinus = (-1f / data.axes[0]);
        }

        if (!isModCStick && data.buttons[11] && data.buttons[7])
        {
            calibrModAnalogXPlus = (1f / data.axes[1]);
        }

        if (!isModCStick && data.buttons[10] && data.buttons[7])
        {
            calibrModAnalogXMinus = (-1f / data.axes[1]);
        }
        #endregion

        #region CStick Calibration

        if (isModCStick && data.buttons[8] && data.buttons[7])
        {
            calibrModcstickYPlus = (1f / data.axes[2]);
        }

        if (isModCStick && data.buttons[9] && data.buttons[7])
        {
            calibrModcstickYMinus = (1f / data.axes[2]);
        }

        if (isModCStick && data.buttons[10] && data.buttons[7])
        {
            calibrModcstickXPlus = (1f / data.axes[3]);
        }

        if (isModCStick && data.buttons[11] && data.buttons[7])
        {
            calibrModcstickXMinus = (1f / data.axes[3]);
        }
        #endregion
    }

    private void readerReadData(ref byte[] e)
    {

        var input1 = GCNUSBFeeder.GCNState.GetState(getFastInput1(ref e));

        //usbReaderTransfer.Reset();

        data.axes[1] = (((float)(input1.analogX - 128)) / 128f); // Clean this up later
        if (input1.analogX <= 10) { data.axes[1] = 0f; }
        if (data.axes[1] > 0.0f) { data.axes[1] *= calibrModAnalogXPlus; }
        else { data.axes[1] *= calibrModAnalogXMinus; }

        data.axes[0] = (((float)(input1.analogY - 128)) / 128f);
        if (input1.analogY <= 10) { data.axes[0] = 0f; }
        if (data.axes[0] > 0.0f) { data.axes[0] *= calibrModAnalogYPlus; }
        else { data.axes[0] *= calibrModAnalogYMinus; }

        data.axes[3] = (((float)(input1.cstickX - 128)) / 128f);
        if (input1.cstickX <= 10) { data.axes[3] = 0f; }
        if (data.axes[3] > 0.0f) { data.axes[3] *= calibrModcstickXPlus; }
        else { data.axes[3] *= calibrModcstickXMinus; }

        data.axes[2] = (((float)(input1.cstickY - 128)) / 128f);
        if (input1.cstickY <= 10) { data.axes[2] = 0f; }
        if (data.axes[2] > 0.0f) { data.axes[2] *= calibrModcstickYPlus; }
        else { data.axes[2] *= calibrModcstickYMinus; }

        data.axes[4] = ((float)input1.analogL) / 128f;
        data.axes[5] = ((float)input1.analogR) / 128f;



        data.buttons[0] = input1.A;
        data.buttons[1] = input1.B;
        data.buttons[2] = input1.X;
        data.buttons[3] = input1.Y;
        data.buttons[4] = input1.Z;
        data.buttons[5] = input1.R;
        data.buttons[6] = input1.L;
        data.buttons[7] = input1.start;

        data.buttons[8] = input1.up;
        data.buttons[9] = input1.down;
        data.buttons[10] = input1.left;
        data.buttons[11] = input1.right;

        //System.Threading.Thread.Sleep(50);

    }

    private void applyDeadzone()
    {
        float minDeadzone = 0.05f;
        float maxDeadzone = 0.95f;

        if (data.axes[0] < minDeadzone && data.axes[0] > -minDeadzone) { data.axes[0] = 0.0f; }
        if (data.axes[0] > maxDeadzone) { data.axes[0] = 1.0f; }
        if (data.axes[0] < -maxDeadzone) { data.axes[0] = -1.0f; }

        if (data.axes[1] < minDeadzone && data.axes[1] > -minDeadzone) { data.axes[1] = 0.0f; }
        if (data.axes[1] > maxDeadzone) { data.axes[1] = 1.0f; }
        if (data.axes[1] < -maxDeadzone) { data.axes[1] = -1.0f; }

    }


    private void OnApplicationQuit()
    {
        usbReaderTransfer.Cancel();
        if (usbReaderTransfer != null)
            usbReaderTransfer.Dispose();
        UsbDevice.Exit();
        GCNAdapter = null;
        gcThread.Abort();
        gcThread = null;
    }

    /*private void OnApplicationQuit()
    {
        //reader.Abort();
        //usbReaderTransfer.Dispose();
        reader.Dispose();
        writer.Dispose();
        reader = null;
        writer = null;
        if(usbReaderTransfer != null)
            usbReaderTransfer.Dispose();
        GCNAdapter.Close();
        UsbDevice.Exit();
        GCNAdapter = null;
        Destroy(this);
    }*/

    #region input parsing
    //Ugly, but faster than linq, at the very least.
    private byte[] getFastInput1(ref byte[] input)
    {
        return new byte[] { input[1], input[2], input[3], input[4], input[5], input[6], input[7], input[8], input[9] };
    }
    private byte[] getFastInput2(ref byte[] input)
    {
        return new byte[] { input[10], input[11], input[12], input[13], input[14], input[15], input[16], input[17], input[18] };
    }
    private byte[] getFastInput3(ref byte[] input)
    {
        return new byte[] { input[19], input[20], input[21], input[22], input[23], input[24], input[25], input[26], input[27] };
    }
    private byte[] getFastInput4(ref byte[] input)
    {
        return new byte[] { input[28], input[29], input[30], input[31], input[32], input[33], input[34], input[35], input[36] };
    }
    #endregion
}
