namespace HelloWorld
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public interface IGamerFactory
    {
    }

    public interface IPopcornService
    {
    }

    public interface ISweatyPalmsService
    {
    }

    public interface IChineseTakeawayService
    {
    }

    public interface IUnderpantsService
    {
    }

    public interface IGamingHeadsetService
    {
    }

    public class PopcornService : IPopcornService
    {
    }

    public class SweatyPalmsService : ISweatyPalmsService
    {
    }

    public class ChineseTakeawayService : IChineseTakeawayService
    {
    }

    public class UnderpantsService : IUnderpantsService
    {
    }

    public class GamingHeadsetService : IGamingHeadsetService
    {
    }
}