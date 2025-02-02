﻿Public Class ExplorerPatcher
    Public Shared IsInstalled As Boolean = False
    Public UseStart10 As Boolean = False
    Public UseTaskbar10 As Boolean = False
    Public TaskbarButton10 As Boolean = False
    Public StartStyle As StartStyles

    Enum StartStyles
        NotRounded
        RoundedCornersFloatingMenu
        RoundedCornersDockedMenu
    End Enum

    Sub New()

        Try
            If My.Computer.Registry.CurrentUser.OpenSubKey("Software\ExplorerPatcher") IsNot Nothing Then
                IsInstalled = True
            Else
                IsInstalled = False
            End If
        Finally
            My.Computer.Registry.CurrentUser.Close()
        End Try

        If Not My.Settings.ExplorerPatcher.Enabled_Force Then

            If IsInstalled And My.W11 Then
                UseStart10 = Reg_IO.GetReg("Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_ShowClassicMode", False)
                Try
                    With My.Computer.Registry.CurrentUser.OpenSubKey("Software\ExplorerPatcher")
                        UseTaskbar10 = .GetValue("OldTaskbar")
                        TaskbarButton10 = .GetValue("OrbStyle") = 0
                        StartStyle = .GetValue("StartUI_EnableRoundedCorners")
                    End With
                Finally
                    My.Computer.Registry.CurrentUser.Close()
                End Try
            Else
                UseStart10 = False
                UseTaskbar10 = False
                TaskbarButton10 = False
                StartStyle = StartStyles.NotRounded
            End If

        Else
            UseStart10 = My.Settings.ExplorerPatcher.UseStart10
            UseTaskbar10 = My.Settings.ExplorerPatcher.UseTaskbar10
            TaskbarButton10 = My.Settings.ExplorerPatcher.TaskbarButton10
            StartStyle = My.Settings.ExplorerPatcher.StartStyle
        End If


    End Sub

    Public Shared Function IsAllowed() As Boolean
        Dim condition0 As Boolean = My.W11 AndAlso My.Settings.ExplorerPatcher.Enabled AndAlso IsInstalled
        Dim condition1 As Boolean = My.Settings.ExplorerPatcher.Enabled_Force

        Return condition0 OrElse condition1
    End Function
End Class
