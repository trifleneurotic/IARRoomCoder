using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using RoomCoder.Application.Database;
using RoomCoder.Application.Models;
using Spark.Library.Extensions;
using AsyncBridge;

namespace RoomCoder.Application.Services;

public class CurrentCodeNumbersService
{
    private const int RoomCount = 25;

    private readonly DatabaseContext _db;

    private static SortedDictionary<byte, byte>? _orderedCurrentCodeNumbers;
    public SortedDictionary<byte, byte> OrderedCurrentCodeNumbers
    {
        get
        {
            using (var A = AsyncHelper.Wait)
            {
             A.Run(this.GetCurrentCodeNumbersAsync());
            }
            return _orderedCurrentCodeNumbers;
        }
    }

    public async Task CycleCurrentCodeNumber(byte roomNumber)
    {
        var currentCodeRecord = (CurrentCode)await _db.CurrentCodes.FirstAsync(x => x.RoomNumber == roomNumber);
        currentCodeRecord.CurrentCodeNumber += 1;
        _db.CurrentCodes.Save<CurrentCode>(currentCodeRecord);

    }

    public async Task ResetCurrentCodeNumber(byte roomNumber)
    {
        var currentCodeRecord = (CurrentCode)await _db.CurrentCodes.FirstAsync(x => x.RoomNumber == roomNumber);
        currentCodeRecord.CurrentCodeNumber = 1;
        _db.CurrentCodes.Save<CurrentCode>(currentCodeRecord);
    }

    public CurrentCodeNumbersService(DatabaseContext db)
    {
        _db = db;

        if (!_db.CurrentCodes.Any())
        {
            for (int i = 0; i < RoomCount; i++)
            {
                var newCurrentCodeRecord = new CurrentCode();
                newCurrentCodeRecord.RoomNumber = (byte)(i + 1);
                newCurrentCodeRecord.CurrentCodeNumber = 1;
                _db.CurrentCodes.Save<CurrentCode>(newCurrentCodeRecord);
            }
        }
    }

    public async Task GetCurrentCodeNumbersAsync()
    {
        _orderedCurrentCodeNumbers = new SortedDictionary<byte, byte>();

        for (int i = 0; i < RoomCount; i++)
        {
            var currentCodeRecord = (CurrentCode)await _db.CurrentCodes.FirstAsync(x => x.RoomNumber == (byte)(i + 1));
            _orderedCurrentCodeNumbers.TryAdd(currentCodeRecord.RoomNumber, currentCodeRecord.CurrentCodeNumber);
        }
    }
}
