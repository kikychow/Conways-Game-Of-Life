Public Class UserGuideForm
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Me.ClientSize = New Size(800, 800)

        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.ControlBox = False
        Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        Me.BackgroundImage = Image.FromFile("bg.png")
        Me.Text = "About the simulator"

        Dim LabelRules As New Label
        LabelRules.Text = "
Left mouse click: Change the state of the clicked cell
Left mouse drag: Move around

Buttons:
RUN/ STOP : Start or Stop the simulation at custom speed
STEP : Go to the next generation
CLEAR : Reset the grid
REWIND : Load the initial pattern
UNDO : Go back to the previous generation
SKIP : Skip a number of generations
Export : Save the pattern on the grid 
Import : Load the selected pattern on grid
Randomise : Generate a random pattern

Colour settings:
Live cell : Change the colour of the live cells
Dead cell : Change the colour of the dead cells
Border : Change the colour of the border

Track bars:
Speed : Change the time interval between generations
Scale : Change the scale of the grid
Border Width : Change the border width

Rules settings:
Enter a number list, e.g. 1357 represents 1, 3, 5 or 7 neighbours
Numbers should be in the range of 0-8

Copy and Paste:
When the select checkbox is checked:
Left mouse drag : Select a region
Copy : Copy the selected region
Paste : Paste the previously selected region onto the new selected region
"
        LabelRules.BackColor = Color.FromArgb(200, Color.Gold)
        LabelRules.Padding = New System.Windows.Forms.Padding(20)
        LabelRules.Width = 700
        LabelRules.Height = 700
        LabelRules.TextAlign = ContentAlignment.MiddleLeft
        LabelRules.Font = New Font("Cambria", 12)
        LabelRules.Location = New Point((Me.Width - LabelRules.Width) / 2, 20)
        Me.Controls.Add(LabelRules)

        Dim ButtonBack As New Button
        ButtonBack.Text = "Back"
        'ButtonBack.BorderStyle = BorderStyle.Fixed3D
        'ButtonBack.BackColor = Color.FromArgb(220, Color.White)
        ButtonBack.Width = 50
        ButtonBack.Height = 30
        ButtonBack.TextAlign = ContentAlignment.MiddleCenter
        ButtonBack.Font = New Font("Cambria", 10)
        ButtonBack.Location = New Point((Me.Width - ButtonBack.Width) / 2, 750)
        ButtonBack.BackColor = Color.White
        ButtonBack.ForeColor = Color.Gold
        ButtonBack.FlatStyle = False
        AddHandler ButtonBack.Click, AddressOf ButtonBack_Click
        Me.Controls.Add(ButtonBack)

    End Sub

    Protected Sub ButtonBack_Click(sender As Object, e As EventArgs)
        Me.Hide()
    End Sub
End Class