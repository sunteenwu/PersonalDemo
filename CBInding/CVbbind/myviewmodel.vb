Public Class myviewmodel
    Implements INotifyPropertyChanged
    Private Sub NotifyPropertyChanged(Optional propertyName As String = "")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Private _myrectangles As New ObservableCollection(Of RectItem)
    Public Property myrectangles As ObservableCollection(Of RectItem)
        Get
            Return _myrectangles
        End Get
        Set(value As ObservableCollection(Of RectItem))
            _myrectangles = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private m_sendCommand As IDelegateCommand
    Public Property SendMyDC As IDelegateCommand
        Get
            Return m_sendCommand

        End Get
        Protected Set(value As IDelegateCommand)
            m_sendCommand = value
        End Set
    End Property


    Public Sub New()
        Me.SendMyDC = New DelegateCommand(AddressOf ExecuteSendMyDC)
        Dim newrect As New RectItem
        newrect.Height = 100
        newrect.Width = 150
        newrect.X = 50
        newrect.Y = 50
        _myrectangles.Add(newrect)
    End Sub

    Dim selrecitem As RectItem
    Public Sub PointerDrag(sender As Object, e As ManipulationDeltaRoutedEventArgs)
        Dim dx_point = e.Delta.Translation.X
        NotifyPropertyChanged()
        Dim dy_point = e.Delta.Translation.Y
        NotifyPropertyChanged()
        'Dim rec = TryCast(e.OriginalSource, Button)
        'Dim selrecitem = TryCast(rec.DataContext, RectItem)
        If selrecitem IsNot Nothing Then
            selrecitem.X += dx_point
            selrecitem.Y += dy_point
        End If
    End Sub

#Region "Execute and CanExecute methods"

    Private Sub ExecuteSendMyDC(param As Object)
        selrecitem = CType(param, RectItem)
    End Sub

#End Region

End Class
