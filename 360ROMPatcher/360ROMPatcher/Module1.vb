Imports System.IO
Module Module1
    Public cmdArgs() As String
    Sub Main()

        'About
        Console.Clear()
        Console.WriteLine("360ROMPatcher v1.0 by Kirby0Louise")
        Console.WriteLine()
        Console.WriteLine("Use -h for help")

        'Check xdetla and beatv2
        If Not File.Exists("xdelta.exe") Then
            errorExit("Error - xdelta not found!")
        End If

        cmdArgs = Environment.GetCommandLineArgs()

        If cmdArgs.Length = 1 Then
            'Ran without any args
        ElseIf cmdArgs.Length = 2 Then
            If cmdArgs(1) = "-h" Then
                'Show help
                Console.WriteLine("Usage:")
                Console.WriteLine("360ROMPatcher <PATCH FILE> <ROM ROOT>")
            Else
                'Did something weird?
                errorExit("ERROR - You did something weird.  Probably only provided a single path.")
            End If
        ElseIf cmdArgs.Length = 3 Then
            'Correct argument length
            If Not cmdArgs(1).ToUpper().EndsWith(".360PATCH") Then
                errorExit("Error - You didn't specify a .360PATCH for <PATCH FILE>")
            End If

            If Not Directory.Exists(cmdArgs(2)) Then
                errorExit("Error - <ROM ROOT> doesn't exist or you don't have permission to access it")
            End If


            'All good, go and patch ROM
            patchFile(cmdArgs(1), cmdArgs(2))

        Else
            'too many args
            errorExit("ERROR - TOO MANY ARGUMENTS")
        End If
    End Sub


    Sub patchFile(ByVal patchPath As String, ByVal targetDir As String)
        'Read all patch file commands in
        Dim commands As String() = File.ReadAllLines(patchPath)

        For Each line In commands
            If line = "360PATCHERMAGIC" Then
                'Do nothing, just for organization
            ElseIf line = "" Then
                'Do nothing
            ElseIf line = "360PATCHEREOF" Then
                'EOF
                Console.WriteLine("Finished patching!")
                Console.WriteLine("Press any key to escape")
                Console.ReadKey()
                Environment.Exit(0)
            Else
                'Decode command
                Dim cmds As String() = line.Split("|")

                Dim targetFile As String = targetDir + cmds(1)

                Console.WriteLine("Patching " + targetFile + " with " + cmds(0))

                'xdelta can't overwrite, have to do hacky rename workaround
                'Dim tFilename As String = targetFile.Substring(targetFile.LastIndexOf("."))

                'Patch the damn thing!
                Dim startArgs As String = "-d -s " + targetFile + " " + cmds(0) + " " + targetFile + "[P]"
                Console.WriteLine(Environment.CurrentDirectory + "\xdelta.exe " + startArgs)
                Console.WriteLine()
                Process.Start(Environment.CurrentDirectory + "\xdelta.exe", startArgs)
                'an ounce of prevention....
                Threading.Thread.Sleep(5000)
                File.Delete(targetFile)
                File.Move(targetFile + "[P]", targetFile)
            End If
        Next


    End Sub

    Sub errorExit(ByVal errorString As String)
        Console.WriteLine(errorString)
        Console.WriteLine("Press any key to escape")
        Console.ReadKey()
        Environment.Exit(0)
    End Sub

End Module
