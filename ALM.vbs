Set objTDConnection = CreateObject("TDApiOle80.TDConnection")

' ALM server URL
ALMServer = "http://YourALMServer/qcbin"

' User credentials
UserName = "YourUserName"
Password = "YourPassword"
Domain = "YourDomain"
Project = "YourProject"

' Connect to ALM
objTDConnection.InitConnectionEx ALMServer
objTDConnection.Login UserName, Password
objTDConnection.Connect Domain, Project

' Check if the connection is successful
If objTDConnection.Connected Then
    WScript.Echo "Connected to ALM."

    ' Test Set details
    TestSetFolder = "Root\TestSetFolder"
    TestSetName = "YourTestSetName"

    ' Find the test set
    Set testSetTreeManager = objTDConnection.TestSetTreeManager
    Set testSetFolder = testSetTreeManager.NodeByPath(TestSetFolder)
    Set testSet = testSetFolder.FindTestSet(TestSetName)

    If Not testSet Is Nothing Then
        WScript.Echo "Found Test Set: " & TestSetName

        ' Attach a file to the test set
        AttachmentFilePath = "C:\Path\To\Your\File.txt"
        Set attachmentFactory = testSet.Attachments
        Set attachment = attachmentFactory.AddItem(Null)
        attachment.FileName = AttachmentFilePath
        attachment.Post

        WScript.Echo "Attachment uploaded to Test Set: " & TestSetName

        ' Trigger execution
        Set runFactory = testSet.RunFactory
        Set run = runFactory.AddItem(Null)
        run.Status = "Not Completed"
        run.Post

        WScript.Echo "Execution triggered for Test Set: " & TestSetName
    Else
        WScript.Echo "Test Set not found: " & TestSetName
    End If

    ' Logout and disconnect
    objTDConnection.Logout
    objTDConnection.Disconnect
Else
    WScript.Echo "Failed to connect to ALM."
End If
