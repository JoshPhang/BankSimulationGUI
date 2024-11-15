Imports System.IO

Module Program
    Private file_name As String
    Private reader As StreamReader

    Private eventQueue As New EventQueue
    Private arrivalQueue As New CustomerQueue
    Private customerQueue As New CustomerQueue
    Private serveQueue As New CustomerQueue
    Private summaryQueue As New CustomerQueue

    Sub Main(args As String())
        ' Get input file name from user
        Console.WriteLine("Please enter file name...")
        file_name = Console.ReadLine()
        If File.Exists(file_name) = False Then
            Console.WriteLine("File does not exist. Program shutting down.")
            Return
        Else
            reader = New StreamReader(file_name)
        End If

        ' Read first line from file and start first arrival
        Dim customer As Customer
        Dim nextServeTime = 0
        Dim ready = True
        Dim last_id = 100

        customer = getNextCustomer()
        arrivalQueue.push(customer)
        eventQueue.push(New [Event](0, customer.arrTime))

        ' Begin Simulation Loop
        While (Not eventQueue.isEmpty())
            Dim currEvent = eventQueue.pop()
            Select Case currEvent.event_id
                Case 0 ' Arrival event (0)
                    If (Not arrivalQueue.isEmpty()) Then ' Check for customer arrival
                        customer = arrivalQueue.peek()
                        If (Not customer.hasArrived And customer.arrTime = currEvent.time) Then
                            customer = arrivalQueue.pop()
                            customer.id = last_id
                            last_id += 1
                            Console.WriteLine($"Time {currEvent.time}: Customer {customer.id} has arrived with serve time {customer.serveTime}")
                            customer.hasArrived = True
                            customerQueue.push(customer)
                        End If
                    End If
                    If (Not customerQueue.isEmpty()) Then ' Check if customer is ready to begin transaction
                        customer = customerQueue.peek()
                        If ready Then
                            ready = False ' Block customers from beginning transaction
                            customer = customerQueue.pop()
                            eventQueue.push(New [Event](1, currEvent.time + customer.serveTime))
                            customer.waitTime = currEvent.time - customer.arrTime
                            Console.WriteLine($"Time {currEvent.time}: Customer {customer.id} has begun transaction (waited {customer.waitTime}s)")
                            serveQueue.push(customer)
                        End If
                    End If
                    If (Not reader.EndOfStream) Then ' Check if a new arrival can occur
                        customer = getNextCustomer()
                        arrivalQueue.push(customer)
                        eventQueue.push(New [Event](0, customer.arrTime))
                    End If

                Case 1 ' Departure event (1)
                    ' Process the departure of the current customer in service
                    If Not serveQueue.isEmpty() Then
                        customer = serveQueue.pop()
                        customer.departTime = currEvent.time
                        summaryQueue.push(customer)
                        Console.WriteLine($"Time {currEvent.time}: Customer {customer.id} has finished their transaction")

                        ' See if there are any other customers in line
                        If Not customerQueue.isEmpty() Then
                            ' Begin service for the next customer in line
                            Dim upcomingCustomer As Customer = customerQueue.pop()
                            upcomingCustomer.waitTime = currEvent.time - upcomingCustomer.arrTime
                            eventQueue.push(New [Event](1, currEvent.time + upcomingCustomer.serveTime))
                            Console.WriteLine($"Time {currEvent.time}: Customer {upcomingCustomer.id} started transaction (waited {upcomingCustomer.waitTime}s)")
                            serveQueue.push(upcomingCustomer)
                        Else
                            ' If no one is waiting, mark the teller as available
                            ready = True
                        End If
                    End If

            End Select

        End While

        ' Close file reader
        reader.Close()

        printSummary()
    End Sub

    Function getNextCustomer() As Customer
        Dim input() As String = reader.ReadLine().Split
        Dim arrTime As Integer = Convert.ToInt32(input(0))
        Dim serveTime As Integer = Convert.ToInt32(input(1))
        Return New Customer(arrTime, serveTime)
    End Function

    Function printSummary()
        Console.WriteLine(Environment.NewLine)
        Console.WriteLine($"ID{ControlChars.Tab}Arrival{ControlChars.Tab}Service{ControlChars.Tab}Wait{ControlChars.Tab}Depart")
        For i As Integer = 0 To summaryQueue.Queue.Count - 1
            Dim customer = summaryQueue.pop()
            Console.WriteLine($"{customer.id}{ControlChars.Tab}{customer.arrTime}{ControlChars.Tab}{customer.serveTime}{ControlChars.Tab}{customer.waitTime}{ControlChars.Tab}{customer.departTime}")
        Next i
        Return 0
    End Function


End Module
