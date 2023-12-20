using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.y2023;

[AoC(2023)]
public class Day20 : Day
{
    private const string BroadcastModuleName = "broadcaster";
    private const string ButtonModuleName = "button";
    private const string ResetModuleName = "rx";
    public Day20() : base(20, 2023)
    {

    }

    public override object Part1(List<string> input)
    {
        var modules = SetupModules(input);

        for (int i = 0; i < 1000; i++)
            ButtonPress(modules);

        return modules.Values.Aggregate((Low: 0L, High: 0L), (data, next) => (data.Low+=next.LowPulsesSent, data.High+next.HighPulsesSent), data => data.Low * data.High);
    }

    public override object Part2(List<string> input)
    {
        var modules = SetupModules(input);
        if(!modules.TryGetValue(ResetModuleName, out var rx) || rx.InputModules.Count != 1)
            return 0;
        
        var affectingModules = rx.InputModules[0].InputModules;
        var loopsUntilHigh = affectingModules.ToDictionary(m => m.Name, _ => 0);
        var count = 1;
        do
        {
            ButtonPress(modules);
            affectingModules.Where(m => m.HighPulsesSent > 0 && loopsUntilHigh[m.Name] == 0).ForEach(m => loopsUntilHigh[m.Name] = count);
            count++;
        } while (loopsUntilHigh.Values.Any(v => v == 0));

        return loopsUntilHigh.Values.Aggregate(1L, (value, v) => value * v);
    }

    private static void ButtonPress(Dictionary<string, Module> modules)
    {
        var queue = new Queue<Module>();
        var module = modules[ButtonModuleName];
        module.EnqueueSignal(false, module.Name);
        do
        {
            module.ProcessSignal().ForEach(queue.Enqueue);
        } while (queue.TryDequeue(out module));
    }

    private static Dictionary<string, Module> SetupModules(List<string> input)
    {
        var data = input.Select(r => r.Split(" -> ")).ToList();
        var modules = data.Select(d => Module.Create(d[0])).ToDictionary(m => m.Name, m => m);
        data.ForEach(d =>
        {
            var connectedModules = d[1].Split(',', ' ', StringSplitOptions.TrimEntries);
            var name = d[0][0] is '%' or '&' ? d[0][1..] : d[0];
            connectedModules.ForEach(m => 
            {
                if(!modules.ContainsKey(m))
                    modules[m] = new OutputModule(m);
                modules[name].AddOutputModule(modules[m]);
                modules[m].AddInputModule(modules[name]);
            });
        });
        var buttonModule = new ButtonModule(ButtonModuleName);
        buttonModule.AddOutputModule(modules[BroadcastModuleName]);
        modules[buttonModule.Name] = buttonModule;

        return modules;
    }

    private class ButtonModule(string name) : Module(name) {}
    private class BroadcastModule(string name) : Module(name) {}
    private class OutputModule(string name) : Module(name) 
    {
        protected override bool ShouldHandleSignal(bool highSignal) => false;
    }
    private class FlipFlopModule(string name) : Module(name)
    {
        protected override bool ShouldHandleSignal(bool highSignal) => !highSignal;

        protected override bool TransformSignal(bool signal) => state = !state;
    }
    private class ConjunctionModule(string name) : Module(name)
    {
        readonly Dictionary<string, bool> sources = [];
        public override void AddInputModule(Module module)
        {
            sources[module.Name] = false;
            base.AddInputModule(module);
        }
        public override void EnqueueSignal(bool signal, string source)
        {
            sources[source] = signal;
            base.EnqueueSignal(signal, source);
        }
        protected override bool TransformSignal(bool signal) => !sources.Values.All(high => high);
    }
    private abstract class Module(string name)
    {
        public long LowPulsesSent { get { return lowPulsesSent; }} 
        public long HighPulsesSent { get { return highPulsesSent; }}
        public string Name { get; } = name;
        protected bool state;
        private int lowPulsesSent = 0;
        private int highPulsesSent = 0;
        private readonly Queue<bool> queue = [];
        protected readonly HashSet<Module> outputModules = [];
        protected readonly HashSet<Module> inputModules = [];
        public List<Module> InputModules => [.. inputModules];
        public void AddOutputModule(Module module) => outputModules.Add(module);
        public List<Module> ProcessSignal()
        {
            List<Module> queued = [];
            if(queue.TryDequeue(out var outputSignal) && ShouldHandleSignal(outputSignal))
            {
                var signal = TransformSignal(outputSignal);
                foreach (var module in outputModules)
                {
                    if(signal)
                        highPulsesSent++;
                    else
                        lowPulsesSent++;

                    module.EnqueueSignal(signal, Name);
                    queued.Add(module);
                }
            }
            return queued;
        }
        public virtual void AddInputModule(Module module) => inputModules.Add(module);
        public virtual void EnqueueSignal(bool signal, string source) => queue.Enqueue(signal);
        protected virtual bool ShouldHandleSignal(bool signal) => true;
        protected virtual bool TransformSignal(bool signal) => signal;
        public static Module Create(string input)
        {
            if(input == BroadcastModuleName)
                return new BroadcastModule(BroadcastModuleName);
            if(input == ButtonModuleName)
                return new BroadcastModule(ButtonModuleName);
        
            return input[0] switch
            {
                '%' => new FlipFlopModule(input[1..]),
                '&' => new ConjunctionModule(input[1..]),
                _ => throw new ArgumentException("Invalid input"),
            };
        }
    }
}
