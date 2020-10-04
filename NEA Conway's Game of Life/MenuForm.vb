Public Class MenuForm

    Public main As SimulatorForm

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Height = 600
        Width = 900

        Me.ControlBox = False
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        Me.BackgroundImage = Image.FromFile("bg.png")
        Me.Text = "Main Menu"

        Dim LabelTitle As New Label
        LabelTitle.Text = "Conway's Game of Life"
        LabelTitle.BackColor = Color.FromArgb(220, Color.White)
        LabelTitle.Width = 420
        LabelTitle.Height = 150
        LabelTitle.TextAlign = ContentAlignment.MiddleCenter
        LabelTitle.Font = New Font("Broadway", 40)
        LabelTitle.Location = New Point((Me.Width - LabelTitle.Width) / 2, 80)
        Me.Controls.Add(LabelTitle)

        Dim ButtonEnter As New Button
        ButtonEnter.Text = "Enter the Game"
        ButtonEnter.Font = New Font("Cambria", 14)
        ButtonEnter.Height = 50
        ButtonEnter.Width = 200
        ButtonEnter.Location = New Point((Me.Width - ButtonEnter.Width) / 2, 300)
        ButtonEnter.BackColor = Color.LightCoral
        ButtonEnter.ForeColor = Color.Black
        ButtonEnter.FlatStyle = FlatStyle.Flat
        ButtonEnter.FlatAppearance.BorderSize = 2
        AddHandler ButtonEnter.Click, AddressOf ButtonEnter_Click
        Me.Controls.Add(ButtonEnter)

        Dim ButtonRules As New Button
        ButtonRules.Text = "About"
        ButtonRules.Font = New Font("Cambria", 14)
        ButtonRules.Height = 50
        ButtonRules.Width = 200
        ButtonRules.Location = New Point((Me.Width - ButtonRules.Width) / 2, 360)
        ButtonRules.BackColor = Color.Gold
        ButtonRules.ForeColor = Color.Black
        ButtonRules.FlatStyle = FlatStyle.Flat
        ButtonRules.FlatAppearance.BorderSize = 2
        AddHandler ButtonRules.Click, AddressOf ButtonRules_Click
        Me.Controls.Add(ButtonRules)

        Dim ButtonExit As New Button
        ButtonExit.Text = "Exit"
        ButtonExit.Font = New Font("Cambria", 14)
        ButtonExit.Height = 50
        ButtonExit.Width = 200
        ButtonExit.Location = New Point((Me.Width - ButtonExit.Width) / 2, 420)
        ButtonExit.BackColor = Color.MediumTurquoise
        ButtonExit.ForeColor = Color.Black
        ButtonExit.FlatStyle = FlatStyle.Flat
        ButtonExit.FlatAppearance.BorderSize = 2
        AddHandler ButtonExit.Click, AddressOf ButtonExit_Click
        Me.Controls.Add(ButtonExit)
    End Sub

    Protected Sub ButtonEnter_Click(sender As Object, e As EventArgs)
        ' Objective 1. a.
        Dim n As String

        ' Input validation
        n = Trim(InputBox("Enter the the grid size here (20 to 350)", "Grid Size", "175"))
        If Not IsNumeric(n) Then
            MessageBox.Show("Please enter numbers only.")
        ElseIf Not n = Int(n) Then
            MessageBox.Show("Please enter integers only.")
        ElseIf n < 20 Then
            MessageBox.Show("Please enter a number greater than or equal to 20.")
        ElseIf n > 350 Then
            MessageBox.Show("Pleasee enter a number less than or equal to 350.")
        Else
            main = New SimulatorForm(n) ' Create the simulator
            main.Show()
            Me.Hide() ' Hide the menu screen
        End If
    End Sub

    Protected Sub ButtonRules_Click(sender As Object, e As EventArgs)
        Dim rules As New UserGuideForm
        rules.Show() ' Open the rule form
    End Sub

    Protected Sub ButtonExit_Click(sender As Object, e As EventArgs)
        Me.Close() ' Close the program
    End Sub
End Class