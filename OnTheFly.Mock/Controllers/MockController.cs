using Microsoft.AspNetCore.Mvc;
using Models;
using System.Text.Json;

namespace OnTheFly.Mock.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MockController : ControllerBase
{
    private List<Passenger>? _passengers;
    private List<Flight>? _flights;
    private List<Sale>? _sales;


    public MockController()
    {
        LoadData();
    }


    [HttpGet("/GetFlights")]
    public List<Flight> GetFlight()
    {
        LoadData();
        return _flights;
    }

    [HttpGet("/GetFlights/{id}")]
    public Flight? GetFlight(int id)
    {
        LoadData();
        return _flights.FirstOrDefault(f => f.Id == id);
    }



    [HttpGet("/GetPassengers")]
    public List<Passenger> GetPassenger() => _passengers;

    [HttpGet("/GetPassengers/{cpf}")]
    public Passenger? GetPassenger(string cpf) => _passengers.FirstOrDefault(p => p.CPF == cpf);



    [HttpGet("/GetSales")]
    public List<Sale> GetSale() => _sales;

    [HttpGet("/GetSales/{id}")]
    public Sale? GetSale(int id) => _sales.FirstOrDefault(s => s.Id == id);



    [HttpPut("/UpdateFlight/{id}")]
    public void UpdateFlight(int id, [FromBody] Flight flight)
    {
        var index = _flights.FindIndex(f => f.Id == id);
        _flights[index] = flight;

        System.IO.File.WriteAllText("Mocks/flights.json", JsonSerializer.Serialize(_flights));
    }

    private void LoadData()
    {
        string passengersJson = System.IO.File.ReadAllText(@"Mocks/passengers.json");
        string flightsJson = System.IO.File.ReadAllText(@"Mocks/flights.json");
        string salesJson = System.IO.File.ReadAllText(@"Mocks/sales.json");

        _passengers = JsonSerializer.Deserialize<List<Passenger>>(passengersJson);
        _flights = JsonSerializer.Deserialize<List<Flight>>(flightsJson);
        _sales = JsonSerializer.Deserialize<List<Sale>>(salesJson);
    }

}
