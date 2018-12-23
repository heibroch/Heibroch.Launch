using System.Windows;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace Heibroch.Launch
{
    public partial class ShortcutWindow : Window
    {
        private static ShortcutViewModel mainWindowViewModel;

        public ShortcutWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            PreviewKeyDown += MainWindow_PreviewKeyDown;
            //QueryTextBox.LostFocus += QueryTextBox_LostFocus;
        }

        //private void QueryTextBox_LostFocus(object sender, RoutedEventArgs e) => QueryTextBox.Focus();

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            switch (e.Key)
            {
                case Key.Escape:
                    mainWindowViewModel.LaunchText = "";
                    Close();
                    break;
                case Key.Down:
                    mainWindowViewModel.IncrementSelection(1);
                    break;
                case Key.Up:
                    mainWindowViewModel.IncrementSelection(-1);
                    break;
                case Key.Enter:
                    mainWindowViewModel.ExecuteSelection();
                    mainWindowViewModel.LaunchText = " ";
                    Close();
                    break;
                default: return;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindowViewModel = DataContext as ShortcutViewModel;
            //mainWindowViewModel.LaunchText = "";
            QueryTextBox.Focus();
            QueryTextBox.SelectAll();
        }


        //public enum Keys
        //{
        //    KeyCode = 65535, // 0x0000FFFF
        //    Modifiers = -65536, // -0x00010000
        //    None = 0,
        //    LButton = 1,
        //    RButton = 2,
        //    Cancel = RButton | LButton, // 0x00000003
        //    MButton = 4,
        //    XButton1 = MButton | LButton, // 0x00000005
        //    XButton2 = MButton | RButton, // 0x00000006
        //    Back = 8,
        //    Tab = Back | LButton, // 0x00000009
        //    LineFeed = Back | RButton, // 0x0000000A
        //    Clear = Back | MButton, // 0x0000000C
        //    Return = Clear | LButton, // 0x0000000D
        //    Enter = Return, // 0x0000000D
        //    ShiftKey = 16, // 0x00000010
        //    ControlKey = ShiftKey | LButton, // 0x00000011
        //    Menu = ShiftKey | RButton, // 0x00000012
        //    Pause = Menu | LButton, // 0x00000013
        //    Capital = ShiftKey | MButton, // 0x00000014
        //    CapsLock = Capital, // 0x00000014
        //    KanaMode = CapsLock | LButton, // 0x00000015
        //    HanguelMode = KanaMode, // 0x00000015
        //    HangulMode = HanguelMode, // 0x00000015
        //    JunjaMode = HangulMode | RButton, // 0x00000017
        //    FinalMode = ShiftKey | Back, // 0x00000018
        //    HanjaMode = FinalMode | LButton, // 0x00000019
        //    KanjiMode = HanjaMode, // 0x00000019
        //    Escape = KanjiMode | RButton, // 0x0000001B
        //    IMEConvert = FinalMode | MButton, // 0x0000001C
        //    IMENonconvert = IMEConvert | LButton, // 0x0000001D
        //    IMEAccept = IMEConvert | RButton, // 0x0000001E
        //    IMEAceept = IMEAccept, // 0x0000001E
        //    IMEModeChange = IMEAceept | LButton, // 0x0000001F
        //    Space = 32, // 0x00000020
        //    Prior = Space | LButton, // 0x00000021
        //    PageUp = Prior, // 0x00000021
        //    Next = Space | RButton, // 0x00000022
        //    PageDown = Next, // 0x00000022
        //    End = PageDown | LButton, // 0x00000023
        //    Home = Space | MButton, // 0x00000024
        //    Left = Home | LButton, // 0x00000025
        //    Up = Home | RButton, // 0x00000026
        //    Right = Up | LButton, // 0x00000027
        //    Down = Space | Back, // 0x00000028
        //    Select = Down | LButton, // 0x00000029
        //    Print = Down | RButton, // 0x0000002A
        //    Execute = Print | LButton, // 0x0000002B
        //    Snapshot = Down | MButton, // 0x0000002C
        //    PrintScreen = Snapshot, // 0x0000002C
        //    Insert = PrintScreen | LButton, // 0x0000002D
        //    Delete = PrintScreen | RButton, // 0x0000002E
        //    Help = Delete | LButton, // 0x0000002F
        //    D0 = Space | ShiftKey, // 0x00000030
        //    D1 = D0 | LButton, // 0x00000031
        //    D2 = D0 | RButton, // 0x00000032
        //    D3 = D2 | LButton, // 0x00000033
        //    D4 = D0 | MButton, // 0x00000034
        //    D5 = D4 | LButton, // 0x00000035
        //    D6 = D4 | RButton, // 0x00000036
        //    D7 = D6 | LButton, // 0x00000037
        //    D8 = D0 | Back, // 0x00000038
        //    D9 = D8 | LButton, // 0x00000039
        //    A = 65, // 0x00000041
        //    B = 66, // 0x00000042
        //    C = B | LButton, // 0x00000043
        //    D = 68, // 0x00000044
        //    E = D | LButton, // 0x00000045
        //    F = D | RButton, // 0x00000046
        //    G = F | LButton, // 0x00000047
        //    H = 72, // 0x00000048
        //    I = H | LButton, // 0x00000049
        //    J = H | RButton, // 0x0000004A
        //    K = J | LButton, // 0x0000004B
        //    L = H | MButton, // 0x0000004C
        //    M = L | LButton, // 0x0000004D
        //    N = L | RButton, // 0x0000004E
        //    O = N | LButton, // 0x0000004F
        //    P = 80, // 0x00000050
        //    Q = P | LButton, // 0x00000051
        //    R = P | RButton, // 0x00000052
        //    S = R | LButton, // 0x00000053
        //    T = P | MButton, // 0x00000054
        //    U = T | LButton, // 0x00000055
        //    V = T | RButton, // 0x00000056
        //    W = V | LButton, // 0x00000057
        //    X = P | Back, // 0x00000058
        //    Y = X | LButton, // 0x00000059
        //    Z = X | RButton, // 0x0000005A
        //    LWin = Z | LButton, // 0x0000005B
        //    RWin = X | MButton, // 0x0000005C
        //    Apps = RWin | LButton, // 0x0000005D
        //    Sleep = Apps | RButton, // 0x0000005F
        //    NumPad0 = 96, // 0x00000060
        //    NumPad1 = NumPad0 | LButton, // 0x00000061
        //    NumPad2 = NumPad0 | RButton, // 0x00000062
        //    NumPad3 = NumPad2 | LButton, // 0x00000063
        //    NumPad4 = NumPad0 | MButton, // 0x00000064
        //    NumPad5 = NumPad4 | LButton, // 0x00000065
        //    NumPad6 = NumPad4 | RButton, // 0x00000066
        //    NumPad7 = NumPad6 | LButton, // 0x00000067
        //    NumPad8 = NumPad0 | Back, // 0x00000068
        //    NumPad9 = NumPad8 | LButton, // 0x00000069
        //    Multiply = NumPad8 | RButton, // 0x0000006A
        //    Add = Multiply | LButton, // 0x0000006B
        //    Separator = NumPad8 | MButton, // 0x0000006C
        //    Subtract = Separator | LButton, // 0x0000006D
        //    Decimal = Separator | RButton, // 0x0000006E
        //    Divide = Decimal | LButton, // 0x0000006F
        //    F1 = NumPad0 | ShiftKey, // 0x00000070
        //    F2 = F1 | LButton, // 0x00000071
        //    F3 = F1 | RButton, // 0x00000072
        //    F4 = F3 | LButton, // 0x00000073
        //    F5 = F1 | MButton, // 0x00000074
        //    F6 = F5 | LButton, // 0x00000075
        //    F7 = F5 | RButton, // 0x00000076
        //    F8 = F7 | LButton, // 0x00000077
        //    F9 = F1 | Back, // 0x00000078
        //    F10 = F9 | LButton, // 0x00000079
        //    F11 = F9 | RButton, // 0x0000007A
        //    F12 = F11 | LButton, // 0x0000007B
        //    F13 = F9 | MButton, // 0x0000007C
        //    F14 = F13 | LButton, // 0x0000007D
        //    F15 = F13 | RButton, // 0x0000007E
        //    F16 = F15 | LButton, // 0x0000007F
        //    F17 = 128, // 0x00000080
        //    F18 = F17 | LButton, // 0x00000081
        //    F19 = F17 | RButton, // 0x00000082
        //    F20 = F19 | LButton, // 0x00000083
        //    F21 = F17 | MButton, // 0x00000084
        //    F22 = F21 | LButton, // 0x00000085
        //    F23 = F21 | RButton, // 0x00000086
        //    F24 = F23 | LButton, // 0x00000087
        //    NumLock = F17 | ShiftKey, // 0x00000090
        //    Scroll = NumLock | LButton, // 0x00000091
        //    LShiftKey = F17 | Space, // 0x000000A0
        //    RShiftKey = LShiftKey | LButton, // 0x000000A1
        //    LControlKey = LShiftKey | RButton, // 0x000000A2
        //    RControlKey = LControlKey | LButton, // 0x000000A3
        //    LMenu = LShiftKey | MButton, // 0x000000A4
        //    RMenu = LMenu | LButton, // 0x000000A5
        //    BrowserBack = LMenu | RButton, // 0x000000A6
        //    BrowserForward = BrowserBack | LButton, // 0x000000A7
        //    BrowserRefresh = LShiftKey | Back, // 0x000000A8
        //    BrowserStop = BrowserRefresh | LButton, // 0x000000A9
        //    BrowserSearch = BrowserRefresh | RButton, // 0x000000AA
        //    BrowserFavorites = BrowserSearch | LButton, // 0x000000AB
        //    BrowserHome = BrowserRefresh | MButton, // 0x000000AC
        //    VolumeMute = BrowserHome | LButton, // 0x000000AD
        //    VolumeDown = BrowserHome | RButton, // 0x000000AE
        //    VolumeUp = VolumeDown | LButton, // 0x000000AF
        //    MediaNextTrack = LShiftKey | ShiftKey, // 0x000000B0
        //    MediaPreviousTrack = MediaNextTrack | LButton, // 0x000000B1
        //    MediaStop = MediaNextTrack | RButton, // 0x000000B2
        //    MediaPlayPause = MediaStop | LButton, // 0x000000B3
        //    LaunchMail = MediaNextTrack | MButton, // 0x000000B4
        //    SelectMedia = LaunchMail | LButton, // 0x000000B5
        //    LaunchApplication1 = LaunchMail | RButton, // 0x000000B6
        //    LaunchApplication2 = LaunchApplication1 | LButton, // 0x000000B7
        //    OemSemicolon = MediaStop | Back, // 0x000000BA
        //    Oem1 = OemSemicolon, // 0x000000BA
        //    Oemplus = Oem1 | LButton, // 0x000000BB
        //    Oemcomma = LaunchMail | Back, // 0x000000BC
        //    OemMinus = Oemcomma | LButton, // 0x000000BD
        //    OemPeriod = Oemcomma | RButton, // 0x000000BE
        //    OemQuestion = OemPeriod | LButton, // 0x000000BF
        //    Oem2 = OemQuestion, // 0x000000BF
        //    Oemtilde = 192, // 0x000000C0
        //    Oem3 = Oemtilde, // 0x000000C0
        //    OemOpenBrackets = Oem3 | Escape, // 0x000000DB
        //    Oem4 = OemOpenBrackets, // 0x000000DB
        //    OemPipe = Oem3 | IMEConvert, // 0x000000DC
        //    Oem5 = OemPipe, // 0x000000DC
        //    OemCloseBrackets = Oem5 | LButton, // 0x000000DD
        //    Oem6 = OemCloseBrackets, // 0x000000DD
        //    OemQuotes = Oem5 | RButton, // 0x000000DE
        //    Oem7 = OemQuotes, // 0x000000DE
        //    Oem8 = Oem7 | LButton, // 0x000000DF
        //    OemBackslash = Oem3 | PageDown, // 0x000000E2
        //    Oem102 = OemBackslash, // 0x000000E2
        //    ProcessKey = Oem3 | Left, // 0x000000E5
        //    Packet = ProcessKey | RButton, // 0x000000E7
        //    Attn = Oem102 | CapsLock, // 0x000000F6
        //    Crsel = Attn | LButton, // 0x000000F7
        //    Exsel = Oem3 | D8, // 0x000000F8
        //    EraseEof = Exsel | LButton, // 0x000000F9
        //    Play = Exsel | RButton, // 0x000000FA
        //    Zoom = Play | LButton, // 0x000000FB
        //    NoName = Exsel | MButton, // 0x000000FC
        //    Pa1 = NoName | LButton, // 0x000000FD
        //    OemClear = NoName | RButton, // 0x000000FE
        //    Shift = 65536, // 0x00010000
        //    Control = 131072, // 0x00020000
        //    Alt = 262144, // 0x00040000
        //}

        //public enum Key
        //{
        //    None = 0,
        //    Cancel = 1,
        //    Back = 2,
        //    Tab = 3,
        //    LineFeed = 4,
        //    Clear = 5,
        //    Enter = 6,
        //    Return = 6,
        //    Pause = 7,
        //    Capital = 8,
        //    CapsLock = 8,
        //    HangulMode = 9,
        //    KanaMode = 9,
        //    JunjaMode = 10, // 0x0000000A
        //    FinalMode = 11, // 0x0000000B
        //    HanjaMode = 12, // 0x0000000C
        //    KanjiMode = 12, // 0x0000000C
        //    Escape = 13, // 0x0000000D
        //    ImeConvert = 14, // 0x0000000E
        //    ImeNonConvert = 15, // 0x0000000F
        //    ImeAccept = 16, // 0x00000010
        //    ImeModeChange = 17, // 0x00000011
        //    Space = 18, // 0x00000012
        //    PageUp = 19, // 0x00000013
        //    Prior = 19, // 0x00000013
        //    Next = 20, // 0x00000014
        //    PageDown = 20, // 0x00000014
        //    End = 21, // 0x00000015
        //    Home = 22, // 0x00000016
        //    Left = 23, // 0x00000017
        //    Up = 24, // 0x00000018
        //    Right = 25, // 0x00000019
        //    Down = 26, // 0x0000001A
        //    Select = 27, // 0x0000001B
        //    Print = 28, // 0x0000001C
        //    Execute = 29, // 0x0000001D
        //    PrintScreen = 30, // 0x0000001E
        //    Snapshot = 30, // 0x0000001E
        //    Insert = 31, // 0x0000001F
        //    Delete = 32, // 0x00000020
        //    Help = 33, // 0x00000021
        //    D0 = 34, // 0x00000022
        //    D1 = 35, // 0x00000023
        //    D2 = 36, // 0x00000024
        //    D3 = 37, // 0x00000025
        //    D4 = 38, // 0x00000026
        //    D5 = 39, // 0x00000027
        //    D6 = 40, // 0x00000028
        //    D7 = 41, // 0x00000029
        //    D8 = 42, // 0x0000002A
        //    D9 = 43, // 0x0000002B
        //    A = 44, // 0x0000002C
        //    B = 45, // 0x0000002D
        //    C = 46, // 0x0000002E
        //    D = 47, // 0x0000002F
        //    E = 48, // 0x00000030
        //    F = 49, // 0x00000031
        //    G = 50, // 0x00000032
        //    H = 51, // 0x00000033
        //    I = 52, // 0x00000034
        //    J = 53, // 0x00000035
        //    K = 54, // 0x00000036
        //    L = 55, // 0x00000037
        //    M = 56, // 0x00000038
        //    N = 57, // 0x00000039
        //    O = 58, // 0x0000003A
        //    P = 59, // 0x0000003B
        //    Q = 60, // 0x0000003C
        //    R = 61, // 0x0000003D
        //    S = 62, // 0x0000003E
        //    T = 63, // 0x0000003F
        //    U = 64, // 0x00000040
        //    V = 65, // 0x00000041
        //    W = 66, // 0x00000042
        //    X = 67, // 0x00000043
        //    Y = 68, // 0x00000044
        //    Z = 69, // 0x00000045
        //    LWin = 70, // 0x00000046
        //    RWin = 71, // 0x00000047
        //    Apps = 72, // 0x00000048
        //    Sleep = 73, // 0x00000049
        //    NumPad0 = 74, // 0x0000004A
        //    NumPad1 = 75, // 0x0000004B
        //    NumPad2 = 76, // 0x0000004C
        //    NumPad3 = 77, // 0x0000004D
        //    NumPad4 = 78, // 0x0000004E
        //    NumPad5 = 79, // 0x0000004F
        //    NumPad6 = 80, // 0x00000050
        //    NumPad7 = 81, // 0x00000051
        //    NumPad8 = 82, // 0x00000052
        //    NumPad9 = 83, // 0x00000053
        //    Multiply = 84, // 0x00000054
        //    Add = 85, // 0x00000055
        //    Separator = 86, // 0x00000056
        //    Subtract = 87, // 0x00000057
        //    Decimal = 88, // 0x00000058
        //    Divide = 89, // 0x00000059
        //    F1 = 90, // 0x0000005A
        //    F2 = 91, // 0x0000005B
        //    F3 = 92, // 0x0000005C
        //    F4 = 93, // 0x0000005D
        //    F5 = 94, // 0x0000005E
        //    F6 = 95, // 0x0000005F
        //    F7 = 96, // 0x00000060
        //    F8 = 97, // 0x00000061
        //    F9 = 98, // 0x00000062
        //    F10 = 99, // 0x00000063
        //    F11 = 100, // 0x00000064
        //    F12 = 101, // 0x00000065
        //    F13 = 102, // 0x00000066
        //    F14 = 103, // 0x00000067
        //    F15 = 104, // 0x00000068
        //    F16 = 105, // 0x00000069
        //    F17 = 106, // 0x0000006A
        //    F18 = 107, // 0x0000006B
        //    F19 = 108, // 0x0000006C
        //    F20 = 109, // 0x0000006D
        //    F21 = 110, // 0x0000006E
        //    F22 = 111, // 0x0000006F
        //    F23 = 112, // 0x00000070
        //    F24 = 113, // 0x00000071
        //    NumLock = 114, // 0x00000072
        //    Scroll = 115, // 0x00000073
        //    LeftShift = 116, // 0x00000074
        //    RightShift = 117, // 0x00000075
        //    LeftCtrl = 118, // 0x00000076
        //    RightCtrl = 119, // 0x00000077
        //    LeftAlt = 120, // 0x00000078
        //    RightAlt = 121, // 0x00000079
        //    BrowserBack = 122, // 0x0000007A
        //    BrowserForward = 123, // 0x0000007B
        //    BrowserRefresh = 124, // 0x0000007C
        //    BrowserStop = 125, // 0x0000007D
        //    BrowserSearch = 126, // 0x0000007E
        //    BrowserFavorites = 127, // 0x0000007F
        //    BrowserHome = 128, // 0x00000080
        //    VolumeMute = 129, // 0x00000081
        //    VolumeDown = 130, // 0x00000082
        //    VolumeUp = 131, // 0x00000083
        //    MediaNextTrack = 132, // 0x00000084
        //    MediaPreviousTrack = 133, // 0x00000085
        //    MediaStop = 134, // 0x00000086
        //    MediaPlayPause = 135, // 0x00000087
        //    LaunchMail = 136, // 0x00000088
        //    SelectMedia = 137, // 0x00000089
        //    LaunchApplication1 = 138, // 0x0000008A
        //    LaunchApplication2 = 139, // 0x0000008B
        //    Oem1 = 140, // 0x0000008C
        //    OemSemicolon = 140, // 0x0000008C
        //    OemPlus = 141, // 0x0000008D
        //    OemComma = 142, // 0x0000008E
        //    OemMinus = 143, // 0x0000008F
        //    OemPeriod = 144, // 0x00000090
        //    Oem2 = 145, // 0x00000091
        //    OemQuestion = 145, // 0x00000091
        //    Oem3 = 146, // 0x00000092
        //    OemTilde = 146, // 0x00000092
        //    AbntC1 = 147, // 0x00000093
        //    AbntC2 = 148, // 0x00000094
        //    Oem4 = 149, // 0x00000095
        //    OemOpenBrackets = 149, // 0x00000095
        //    Oem5 = 150, // 0x00000096
        //    OemPipe = 150, // 0x00000096
        //    Oem6 = 151, // 0x00000097
        //    OemCloseBrackets = 151, // 0x00000097
        //    Oem7 = 152, // 0x00000098
        //    OemQuotes = 152, // 0x00000098
        //    Oem8 = 153, // 0x00000099
        //    Oem102 = 154, // 0x0000009A
        //    OemBackslash = 154, // 0x0000009A
        //    ImeProcessed = 155, // 0x0000009B
        //    System = 156, // 0x0000009C
        //    DbeAlphanumeric = 157, // 0x0000009D
        //    OemAttn = 157, // 0x0000009D
        //    DbeKatakana = 158, // 0x0000009E
        //    OemFinish = 158, // 0x0000009E
        //    DbeHiragana = 159, // 0x0000009F
        //    OemCopy = 159, // 0x0000009F
        //    DbeSbcsChar = 160, // 0x000000A0
        //    OemAuto = 160, // 0x000000A0
        //    DbeDbcsChar = 161, // 0x000000A1
        //    OemEnlw = 161, // 0x000000A1
        //    DbeRoman = 162, // 0x000000A2
        //    OemBackTab = 162, // 0x000000A2
        //    Attn = 163, // 0x000000A3
        //    DbeNoRoman = 163, // 0x000000A3
        //    CrSel = 164, // 0x000000A4
        //    DbeEnterWordRegisterMode = 164, // 0x000000A4
        //    DbeEnterImeConfigureMode = 165, // 0x000000A5
        //    ExSel = 165, // 0x000000A5
        //    DbeFlushString = 166, // 0x000000A6
        //    EraseEof = 166, // 0x000000A6
        //    DbeCodeInput = 167, // 0x000000A7
        //    Play = 167, // 0x000000A7
        //    DbeNoCodeInput = 168, // 0x000000A8
        //    Zoom = 168, // 0x000000A8
        //    DbeDetermineString = 169, // 0x000000A9
        //    NoName = 169, // 0x000000A9
        //    DbeEnterDialogConversionMode = 170, // 0x000000AA
        //    Pa1 = 170, // 0x000000AA
        //    OemClear = 171, // 0x000000AB
        //    DeadCharProcessed = 172, // 0x000000AC
        //}
    }
}


//using System;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Windows;
//using System.Windows.Input;
//using Application = System.Windows.Application;
//using KeyEventArgs = System.Windows.Input.KeyEventArgs;

//namespace Heibroch.Launch
//{
//    public partial class ShortcutWindow : Window
//    {
//        private static MainWindowViewModel mainWindowViewModel;

//        public ShortcutWindow()
//        {
//            InitializeComponent();
//            Loaded += MainWindow_Loaded;
//            PreviewKeyDown += MainWindow_PreviewKeyDown;
//            Closing += MainWindow_Closing;

//            var shortcutCollection = new ShortcutCollection();
//            mainWindowViewModel = new MainWindowViewModel(shortcutCollection);
//            DataContext = mainWindowViewModel;

//            _hookID = SetHook(_proc);
//        }

//        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            try { UnhookWindowsHookEx(_hookID); }
//            catch { }
//        }

//        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
//        {
//            switch (e.Key)
//            {
//                case Key.Escape:
//                    mainWindowViewModel.LaunchText = string.Empty;
//                    Visibility = Visibility.Hidden;
//                    break;
//                case Key.Down:
//                    mainWindowViewModel.IncrementSelection(1);
//                    break;
//                case Key.Up:
//                    mainWindowViewModel.IncrementSelection(-1);
//                    break;
//                case Key.Enter:
//                    mainWindowViewModel.ExecuteSelection();
//                    Visibility = Visibility.Hidden;
//                    break;
//                default: return;
//            }
//        }

//        private void MainWindow_Loaded(object sender, RoutedEventArgs e) => QueryTextBox.Focus();

//        //Key intercepting

//        private const int WH_KEYBOARD_LL = 13;
//        private const int WM_KEYDOWN = 0x0100;
//        private static LowLevelKeyboardProc _proc = HookCallback;
//        private static IntPtr _hookID = IntPtr.Zero;

//        private static IntPtr SetHook(LowLevelKeyboardProc proc)
//        {
//            using (Process curProcess = Process.GetCurrentProcess())
//            {
//                using (ProcessModule curModule = curProcess.MainModule)
//                {
//                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
//                }
//            }
//        }

//        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

//        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
//        {
//            if (nCode < 0 || wParam != (IntPtr)WM_KEYDOWN)
//                return CallNextHookEx(_hookID, nCode, wParam, lParam);

//            int vkCode = Marshal.ReadInt32(lParam);

//            if (Keyboard.Modifiers == ModifierKeys.Control && vkCode == 32) //Space
//            {
//                var mainWindow = Application.Current?.MainWindow;
//                if (mainWindow != null)
//                {
//                    mainWindow.Visibility = Visibility.Visible;
//                    mainWindow.Activate();
//                }
//            }

//            return CallNextHookEx(_hookID, nCode, wParam, lParam);
//        }

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

//        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr GetModuleHandle(string lpModuleName);

//        //public enum Keys
//        //{
//        //    KeyCode = 65535, // 0x0000FFFF
//        //    Modifiers = -65536, // -0x00010000
//        //    None = 0,
//        //    LButton = 1,
//        //    RButton = 2,
//        //    Cancel = RButton | LButton, // 0x00000003
//        //    MButton = 4,
//        //    XButton1 = MButton | LButton, // 0x00000005
//        //    XButton2 = MButton | RButton, // 0x00000006
//        //    Back = 8,
//        //    Tab = Back | LButton, // 0x00000009
//        //    LineFeed = Back | RButton, // 0x0000000A
//        //    Clear = Back | MButton, // 0x0000000C
//        //    Return = Clear | LButton, // 0x0000000D
//        //    Enter = Return, // 0x0000000D
//        //    ShiftKey = 16, // 0x00000010
//        //    ControlKey = ShiftKey | LButton, // 0x00000011
//        //    Menu = ShiftKey | RButton, // 0x00000012
//        //    Pause = Menu | LButton, // 0x00000013
//        //    Capital = ShiftKey | MButton, // 0x00000014
//        //    CapsLock = Capital, // 0x00000014
//        //    KanaMode = CapsLock | LButton, // 0x00000015
//        //    HanguelMode = KanaMode, // 0x00000015
//        //    HangulMode = HanguelMode, // 0x00000015
//        //    JunjaMode = HangulMode | RButton, // 0x00000017
//        //    FinalMode = ShiftKey | Back, // 0x00000018
//        //    HanjaMode = FinalMode | LButton, // 0x00000019
//        //    KanjiMode = HanjaMode, // 0x00000019
//        //    Escape = KanjiMode | RButton, // 0x0000001B
//        //    IMEConvert = FinalMode | MButton, // 0x0000001C
//        //    IMENonconvert = IMEConvert | LButton, // 0x0000001D
//        //    IMEAccept = IMEConvert | RButton, // 0x0000001E
//        //    IMEAceept = IMEAccept, // 0x0000001E
//        //    IMEModeChange = IMEAceept | LButton, // 0x0000001F
//        //    Space = 32, // 0x00000020
//        //    Prior = Space | LButton, // 0x00000021
//        //    PageUp = Prior, // 0x00000021
//        //    Next = Space | RButton, // 0x00000022
//        //    PageDown = Next, // 0x00000022
//        //    End = PageDown | LButton, // 0x00000023
//        //    Home = Space | MButton, // 0x00000024
//        //    Left = Home | LButton, // 0x00000025
//        //    Up = Home | RButton, // 0x00000026
//        //    Right = Up | LButton, // 0x00000027
//        //    Down = Space | Back, // 0x00000028
//        //    Select = Down | LButton, // 0x00000029
//        //    Print = Down | RButton, // 0x0000002A
//        //    Execute = Print | LButton, // 0x0000002B
//        //    Snapshot = Down | MButton, // 0x0000002C
//        //    PrintScreen = Snapshot, // 0x0000002C
//        //    Insert = PrintScreen | LButton, // 0x0000002D
//        //    Delete = PrintScreen | RButton, // 0x0000002E
//        //    Help = Delete | LButton, // 0x0000002F
//        //    D0 = Space | ShiftKey, // 0x00000030
//        //    D1 = D0 | LButton, // 0x00000031
//        //    D2 = D0 | RButton, // 0x00000032
//        //    D3 = D2 | LButton, // 0x00000033
//        //    D4 = D0 | MButton, // 0x00000034
//        //    D5 = D4 | LButton, // 0x00000035
//        //    D6 = D4 | RButton, // 0x00000036
//        //    D7 = D6 | LButton, // 0x00000037
//        //    D8 = D0 | Back, // 0x00000038
//        //    D9 = D8 | LButton, // 0x00000039
//        //    A = 65, // 0x00000041
//        //    B = 66, // 0x00000042
//        //    C = B | LButton, // 0x00000043
//        //    D = 68, // 0x00000044
//        //    E = D | LButton, // 0x00000045
//        //    F = D | RButton, // 0x00000046
//        //    G = F | LButton, // 0x00000047
//        //    H = 72, // 0x00000048
//        //    I = H | LButton, // 0x00000049
//        //    J = H | RButton, // 0x0000004A
//        //    K = J | LButton, // 0x0000004B
//        //    L = H | MButton, // 0x0000004C
//        //    M = L | LButton, // 0x0000004D
//        //    N = L | RButton, // 0x0000004E
//        //    O = N | LButton, // 0x0000004F
//        //    P = 80, // 0x00000050
//        //    Q = P | LButton, // 0x00000051
//        //    R = P | RButton, // 0x00000052
//        //    S = R | LButton, // 0x00000053
//        //    T = P | MButton, // 0x00000054
//        //    U = T | LButton, // 0x00000055
//        //    V = T | RButton, // 0x00000056
//        //    W = V | LButton, // 0x00000057
//        //    X = P | Back, // 0x00000058
//        //    Y = X | LButton, // 0x00000059
//        //    Z = X | RButton, // 0x0000005A
//        //    LWin = Z | LButton, // 0x0000005B
//        //    RWin = X | MButton, // 0x0000005C
//        //    Apps = RWin | LButton, // 0x0000005D
//        //    Sleep = Apps | RButton, // 0x0000005F
//        //    NumPad0 = 96, // 0x00000060
//        //    NumPad1 = NumPad0 | LButton, // 0x00000061
//        //    NumPad2 = NumPad0 | RButton, // 0x00000062
//        //    NumPad3 = NumPad2 | LButton, // 0x00000063
//        //    NumPad4 = NumPad0 | MButton, // 0x00000064
//        //    NumPad5 = NumPad4 | LButton, // 0x00000065
//        //    NumPad6 = NumPad4 | RButton, // 0x00000066
//        //    NumPad7 = NumPad6 | LButton, // 0x00000067
//        //    NumPad8 = NumPad0 | Back, // 0x00000068
//        //    NumPad9 = NumPad8 | LButton, // 0x00000069
//        //    Multiply = NumPad8 | RButton, // 0x0000006A
//        //    Add = Multiply | LButton, // 0x0000006B
//        //    Separator = NumPad8 | MButton, // 0x0000006C
//        //    Subtract = Separator | LButton, // 0x0000006D
//        //    Decimal = Separator | RButton, // 0x0000006E
//        //    Divide = Decimal | LButton, // 0x0000006F
//        //    F1 = NumPad0 | ShiftKey, // 0x00000070
//        //    F2 = F1 | LButton, // 0x00000071
//        //    F3 = F1 | RButton, // 0x00000072
//        //    F4 = F3 | LButton, // 0x00000073
//        //    F5 = F1 | MButton, // 0x00000074
//        //    F6 = F5 | LButton, // 0x00000075
//        //    F7 = F5 | RButton, // 0x00000076
//        //    F8 = F7 | LButton, // 0x00000077
//        //    F9 = F1 | Back, // 0x00000078
//        //    F10 = F9 | LButton, // 0x00000079
//        //    F11 = F9 | RButton, // 0x0000007A
//        //    F12 = F11 | LButton, // 0x0000007B
//        //    F13 = F9 | MButton, // 0x0000007C
//        //    F14 = F13 | LButton, // 0x0000007D
//        //    F15 = F13 | RButton, // 0x0000007E
//        //    F16 = F15 | LButton, // 0x0000007F
//        //    F17 = 128, // 0x00000080
//        //    F18 = F17 | LButton, // 0x00000081
//        //    F19 = F17 | RButton, // 0x00000082
//        //    F20 = F19 | LButton, // 0x00000083
//        //    F21 = F17 | MButton, // 0x00000084
//        //    F22 = F21 | LButton, // 0x00000085
//        //    F23 = F21 | RButton, // 0x00000086
//        //    F24 = F23 | LButton, // 0x00000087
//        //    NumLock = F17 | ShiftKey, // 0x00000090
//        //    Scroll = NumLock | LButton, // 0x00000091
//        //    LShiftKey = F17 | Space, // 0x000000A0
//        //    RShiftKey = LShiftKey | LButton, // 0x000000A1
//        //    LControlKey = LShiftKey | RButton, // 0x000000A2
//        //    RControlKey = LControlKey | LButton, // 0x000000A3
//        //    LMenu = LShiftKey | MButton, // 0x000000A4
//        //    RMenu = LMenu | LButton, // 0x000000A5
//        //    BrowserBack = LMenu | RButton, // 0x000000A6
//        //    BrowserForward = BrowserBack | LButton, // 0x000000A7
//        //    BrowserRefresh = LShiftKey | Back, // 0x000000A8
//        //    BrowserStop = BrowserRefresh | LButton, // 0x000000A9
//        //    BrowserSearch = BrowserRefresh | RButton, // 0x000000AA
//        //    BrowserFavorites = BrowserSearch | LButton, // 0x000000AB
//        //    BrowserHome = BrowserRefresh | MButton, // 0x000000AC
//        //    VolumeMute = BrowserHome | LButton, // 0x000000AD
//        //    VolumeDown = BrowserHome | RButton, // 0x000000AE
//        //    VolumeUp = VolumeDown | LButton, // 0x000000AF
//        //    MediaNextTrack = LShiftKey | ShiftKey, // 0x000000B0
//        //    MediaPreviousTrack = MediaNextTrack | LButton, // 0x000000B1
//        //    MediaStop = MediaNextTrack | RButton, // 0x000000B2
//        //    MediaPlayPause = MediaStop | LButton, // 0x000000B3
//        //    LaunchMail = MediaNextTrack | MButton, // 0x000000B4
//        //    SelectMedia = LaunchMail | LButton, // 0x000000B5
//        //    LaunchApplication1 = LaunchMail | RButton, // 0x000000B6
//        //    LaunchApplication2 = LaunchApplication1 | LButton, // 0x000000B7
//        //    OemSemicolon = MediaStop | Back, // 0x000000BA
//        //    Oem1 = OemSemicolon, // 0x000000BA
//        //    Oemplus = Oem1 | LButton, // 0x000000BB
//        //    Oemcomma = LaunchMail | Back, // 0x000000BC
//        //    OemMinus = Oemcomma | LButton, // 0x000000BD
//        //    OemPeriod = Oemcomma | RButton, // 0x000000BE
//        //    OemQuestion = OemPeriod | LButton, // 0x000000BF
//        //    Oem2 = OemQuestion, // 0x000000BF
//        //    Oemtilde = 192, // 0x000000C0
//        //    Oem3 = Oemtilde, // 0x000000C0
//        //    OemOpenBrackets = Oem3 | Escape, // 0x000000DB
//        //    Oem4 = OemOpenBrackets, // 0x000000DB
//        //    OemPipe = Oem3 | IMEConvert, // 0x000000DC
//        //    Oem5 = OemPipe, // 0x000000DC
//        //    OemCloseBrackets = Oem5 | LButton, // 0x000000DD
//        //    Oem6 = OemCloseBrackets, // 0x000000DD
//        //    OemQuotes = Oem5 | RButton, // 0x000000DE
//        //    Oem7 = OemQuotes, // 0x000000DE
//        //    Oem8 = Oem7 | LButton, // 0x000000DF
//        //    OemBackslash = Oem3 | PageDown, // 0x000000E2
//        //    Oem102 = OemBackslash, // 0x000000E2
//        //    ProcessKey = Oem3 | Left, // 0x000000E5
//        //    Packet = ProcessKey | RButton, // 0x000000E7
//        //    Attn = Oem102 | CapsLock, // 0x000000F6
//        //    Crsel = Attn | LButton, // 0x000000F7
//        //    Exsel = Oem3 | D8, // 0x000000F8
//        //    EraseEof = Exsel | LButton, // 0x000000F9
//        //    Play = Exsel | RButton, // 0x000000FA
//        //    Zoom = Play | LButton, // 0x000000FB
//        //    NoName = Exsel | MButton, // 0x000000FC
//        //    Pa1 = NoName | LButton, // 0x000000FD
//        //    OemClear = NoName | RButton, // 0x000000FE
//        //    Shift = 65536, // 0x00010000
//        //    Control = 131072, // 0x00020000
//        //    Alt = 262144, // 0x00040000
//        //}

//        //public enum Key
//        //{
//        //    None = 0,
//        //    Cancel = 1,
//        //    Back = 2,
//        //    Tab = 3,
//        //    LineFeed = 4,
//        //    Clear = 5,
//        //    Enter = 6,
//        //    Return = 6,
//        //    Pause = 7,
//        //    Capital = 8,
//        //    CapsLock = 8,
//        //    HangulMode = 9,
//        //    KanaMode = 9,
//        //    JunjaMode = 10, // 0x0000000A
//        //    FinalMode = 11, // 0x0000000B
//        //    HanjaMode = 12, // 0x0000000C
//        //    KanjiMode = 12, // 0x0000000C
//        //    Escape = 13, // 0x0000000D
//        //    ImeConvert = 14, // 0x0000000E
//        //    ImeNonConvert = 15, // 0x0000000F
//        //    ImeAccept = 16, // 0x00000010
//        //    ImeModeChange = 17, // 0x00000011
//        //    Space = 18, // 0x00000012
//        //    PageUp = 19, // 0x00000013
//        //    Prior = 19, // 0x00000013
//        //    Next = 20, // 0x00000014
//        //    PageDown = 20, // 0x00000014
//        //    End = 21, // 0x00000015
//        //    Home = 22, // 0x00000016
//        //    Left = 23, // 0x00000017
//        //    Up = 24, // 0x00000018
//        //    Right = 25, // 0x00000019
//        //    Down = 26, // 0x0000001A
//        //    Select = 27, // 0x0000001B
//        //    Print = 28, // 0x0000001C
//        //    Execute = 29, // 0x0000001D
//        //    PrintScreen = 30, // 0x0000001E
//        //    Snapshot = 30, // 0x0000001E
//        //    Insert = 31, // 0x0000001F
//        //    Delete = 32, // 0x00000020
//        //    Help = 33, // 0x00000021
//        //    D0 = 34, // 0x00000022
//        //    D1 = 35, // 0x00000023
//        //    D2 = 36, // 0x00000024
//        //    D3 = 37, // 0x00000025
//        //    D4 = 38, // 0x00000026
//        //    D5 = 39, // 0x00000027
//        //    D6 = 40, // 0x00000028
//        //    D7 = 41, // 0x00000029
//        //    D8 = 42, // 0x0000002A
//        //    D9 = 43, // 0x0000002B
//        //    A = 44, // 0x0000002C
//        //    B = 45, // 0x0000002D
//        //    C = 46, // 0x0000002E
//        //    D = 47, // 0x0000002F
//        //    E = 48, // 0x00000030
//        //    F = 49, // 0x00000031
//        //    G = 50, // 0x00000032
//        //    H = 51, // 0x00000033
//        //    I = 52, // 0x00000034
//        //    J = 53, // 0x00000035
//        //    K = 54, // 0x00000036
//        //    L = 55, // 0x00000037
//        //    M = 56, // 0x00000038
//        //    N = 57, // 0x00000039
//        //    O = 58, // 0x0000003A
//        //    P = 59, // 0x0000003B
//        //    Q = 60, // 0x0000003C
//        //    R = 61, // 0x0000003D
//        //    S = 62, // 0x0000003E
//        //    T = 63, // 0x0000003F
//        //    U = 64, // 0x00000040
//        //    V = 65, // 0x00000041
//        //    W = 66, // 0x00000042
//        //    X = 67, // 0x00000043
//        //    Y = 68, // 0x00000044
//        //    Z = 69, // 0x00000045
//        //    LWin = 70, // 0x00000046
//        //    RWin = 71, // 0x00000047
//        //    Apps = 72, // 0x00000048
//        //    Sleep = 73, // 0x00000049
//        //    NumPad0 = 74, // 0x0000004A
//        //    NumPad1 = 75, // 0x0000004B
//        //    NumPad2 = 76, // 0x0000004C
//        //    NumPad3 = 77, // 0x0000004D
//        //    NumPad4 = 78, // 0x0000004E
//        //    NumPad5 = 79, // 0x0000004F
//        //    NumPad6 = 80, // 0x00000050
//        //    NumPad7 = 81, // 0x00000051
//        //    NumPad8 = 82, // 0x00000052
//        //    NumPad9 = 83, // 0x00000053
//        //    Multiply = 84, // 0x00000054
//        //    Add = 85, // 0x00000055
//        //    Separator = 86, // 0x00000056
//        //    Subtract = 87, // 0x00000057
//        //    Decimal = 88, // 0x00000058
//        //    Divide = 89, // 0x00000059
//        //    F1 = 90, // 0x0000005A
//        //    F2 = 91, // 0x0000005B
//        //    F3 = 92, // 0x0000005C
//        //    F4 = 93, // 0x0000005D
//        //    F5 = 94, // 0x0000005E
//        //    F6 = 95, // 0x0000005F
//        //    F7 = 96, // 0x00000060
//        //    F8 = 97, // 0x00000061
//        //    F9 = 98, // 0x00000062
//        //    F10 = 99, // 0x00000063
//        //    F11 = 100, // 0x00000064
//        //    F12 = 101, // 0x00000065
//        //    F13 = 102, // 0x00000066
//        //    F14 = 103, // 0x00000067
//        //    F15 = 104, // 0x00000068
//        //    F16 = 105, // 0x00000069
//        //    F17 = 106, // 0x0000006A
//        //    F18 = 107, // 0x0000006B
//        //    F19 = 108, // 0x0000006C
//        //    F20 = 109, // 0x0000006D
//        //    F21 = 110, // 0x0000006E
//        //    F22 = 111, // 0x0000006F
//        //    F23 = 112, // 0x00000070
//        //    F24 = 113, // 0x00000071
//        //    NumLock = 114, // 0x00000072
//        //    Scroll = 115, // 0x00000073
//        //    LeftShift = 116, // 0x00000074
//        //    RightShift = 117, // 0x00000075
//        //    LeftCtrl = 118, // 0x00000076
//        //    RightCtrl = 119, // 0x00000077
//        //    LeftAlt = 120, // 0x00000078
//        //    RightAlt = 121, // 0x00000079
//        //    BrowserBack = 122, // 0x0000007A
//        //    BrowserForward = 123, // 0x0000007B
//        //    BrowserRefresh = 124, // 0x0000007C
//        //    BrowserStop = 125, // 0x0000007D
//        //    BrowserSearch = 126, // 0x0000007E
//        //    BrowserFavorites = 127, // 0x0000007F
//        //    BrowserHome = 128, // 0x00000080
//        //    VolumeMute = 129, // 0x00000081
//        //    VolumeDown = 130, // 0x00000082
//        //    VolumeUp = 131, // 0x00000083
//        //    MediaNextTrack = 132, // 0x00000084
//        //    MediaPreviousTrack = 133, // 0x00000085
//        //    MediaStop = 134, // 0x00000086
//        //    MediaPlayPause = 135, // 0x00000087
//        //    LaunchMail = 136, // 0x00000088
//        //    SelectMedia = 137, // 0x00000089
//        //    LaunchApplication1 = 138, // 0x0000008A
//        //    LaunchApplication2 = 139, // 0x0000008B
//        //    Oem1 = 140, // 0x0000008C
//        //    OemSemicolon = 140, // 0x0000008C
//        //    OemPlus = 141, // 0x0000008D
//        //    OemComma = 142, // 0x0000008E
//        //    OemMinus = 143, // 0x0000008F
//        //    OemPeriod = 144, // 0x00000090
//        //    Oem2 = 145, // 0x00000091
//        //    OemQuestion = 145, // 0x00000091
//        //    Oem3 = 146, // 0x00000092
//        //    OemTilde = 146, // 0x00000092
//        //    AbntC1 = 147, // 0x00000093
//        //    AbntC2 = 148, // 0x00000094
//        //    Oem4 = 149, // 0x00000095
//        //    OemOpenBrackets = 149, // 0x00000095
//        //    Oem5 = 150, // 0x00000096
//        //    OemPipe = 150, // 0x00000096
//        //    Oem6 = 151, // 0x00000097
//        //    OemCloseBrackets = 151, // 0x00000097
//        //    Oem7 = 152, // 0x00000098
//        //    OemQuotes = 152, // 0x00000098
//        //    Oem8 = 153, // 0x00000099
//        //    Oem102 = 154, // 0x0000009A
//        //    OemBackslash = 154, // 0x0000009A
//        //    ImeProcessed = 155, // 0x0000009B
//        //    System = 156, // 0x0000009C
//        //    DbeAlphanumeric = 157, // 0x0000009D
//        //    OemAttn = 157, // 0x0000009D
//        //    DbeKatakana = 158, // 0x0000009E
//        //    OemFinish = 158, // 0x0000009E
//        //    DbeHiragana = 159, // 0x0000009F
//        //    OemCopy = 159, // 0x0000009F
//        //    DbeSbcsChar = 160, // 0x000000A0
//        //    OemAuto = 160, // 0x000000A0
//        //    DbeDbcsChar = 161, // 0x000000A1
//        //    OemEnlw = 161, // 0x000000A1
//        //    DbeRoman = 162, // 0x000000A2
//        //    OemBackTab = 162, // 0x000000A2
//        //    Attn = 163, // 0x000000A3
//        //    DbeNoRoman = 163, // 0x000000A3
//        //    CrSel = 164, // 0x000000A4
//        //    DbeEnterWordRegisterMode = 164, // 0x000000A4
//        //    DbeEnterImeConfigureMode = 165, // 0x000000A5
//        //    ExSel = 165, // 0x000000A5
//        //    DbeFlushString = 166, // 0x000000A6
//        //    EraseEof = 166, // 0x000000A6
//        //    DbeCodeInput = 167, // 0x000000A7
//        //    Play = 167, // 0x000000A7
//        //    DbeNoCodeInput = 168, // 0x000000A8
//        //    Zoom = 168, // 0x000000A8
//        //    DbeDetermineString = 169, // 0x000000A9
//        //    NoName = 169, // 0x000000A9
//        //    DbeEnterDialogConversionMode = 170, // 0x000000AA
//        //    Pa1 = 170, // 0x000000AA
//        //    OemClear = 171, // 0x000000AB
//        //    DeadCharProcessed = 172, // 0x000000AC
//        //}
//    }
//}
