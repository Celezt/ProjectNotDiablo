using System;

[Flags]
public enum MovementType
{
    None = 0b_0000_0000,
    Forward = 0b_0000_0001,
    Backward = 0b_0000_0010,
    Side = 0b_0000_0100,
}
