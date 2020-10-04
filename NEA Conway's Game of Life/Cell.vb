Public Class Cell

    Private _size As Integer
    Private _borderWidth As Integer
    Private _positionX As Integer
    Private _positionY As Integer

    Private _initialState As Boolean
    Private _currentState As Boolean
    Private _nextState As Boolean
    Private _undoState As New Stack

    Private _cellColour As Color = Color.Black
    Private _deadCellColour As Color = Color.White

    Public Sub New(ByVal size As Integer, ByVal borderWidth As Integer, ByVal x As Integer, ByVal y As Integer)
        _size = size
        _borderWidth = borderWidth
        _positionX = x * _size
        _positionY = y * _size
        _currentState = False
    End Sub

    Public Sub changeState()
        _currentState = Not _currentState
    End Sub

    ' This method paints a cell at its corresponding position, size and colour
    Public Sub drawCell(myGraphics As Graphics)
        Dim brushColour As Color = getCellColour(_currentState)
        Dim cellSize As Integer = _size - _borderWidth
        Using myBrush As SolidBrush = New SolidBrush(brushColour)
            myGraphics.FillRectangle(myBrush, _positionX, _positionY, cellSize, cellSize)
        End Using
    End Sub

    ' This method returns a colour depending on the state of the cell
    Public Function getCellColour(ByVal State As Boolean) As Color
        If State Then
            Return _cellColour
        Else
            Return _deadCellColour
        End If
    End Function

    Public Property size As Integer
        Get
            Return _size
        End Get
        Set(value As Integer)
            _size = value
        End Set
    End Property

    Public Property borderWidth As Integer
        Get
            Return _borderWidth
        End Get
        Set(value As Integer)
            _borderWidth = value
        End Set
    End Property

    Public Property cellColour As Color
        Get
            Return _cellColour
        End Get
        Set(value As Color)
            _cellColour = value
        End Set
    End Property

    Public Property deadCellColour As Color
        Get
            Return _deadcellColour
        End Get
        Set(value As Color)
            _deadcellColour = value
        End Set
    End Property


    Public Property initialState As Boolean
        Get
            Return _initialState
        End Get
        Set(value As Boolean)
            _initialState = value
        End Set
    End Property

    Public Property currentState As Boolean
        Get
            Return _currentState
        End Get
        Set(value As Boolean)
            _currentState = value
        End Set
    End Property

    Public Property nextState As Boolean
        Get
            Return _nextState
        End Get
        Set(value As Boolean)
            _nextState = value
        End Set
    End Property

    Public Property posX As Integer
        Get
            Return _positionX
        End Get
        Set(value As Integer)
            _positionX = value
        End Set
    End Property

    Public Property posY As Integer
        Get
            Return _positionY
        End Get
        Set(value As Integer)
            _positionY = value
        End Set
    End Property

    Public Property undoState As Stack
        Get
            Return _undoState
        End Get
        Set(value As Stack)
            _undoState = value
        End Set
    End Property
End Class