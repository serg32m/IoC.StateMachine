﻿using Microsoft.Practices.Unity;
using IoC.StateMachine.Core;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Tests
{
    [Serializable]
    public class TestContext : ContextBase
    {
        public string TestPropStr { get; set; }
        public int TestPropInt { get; set; }
        public int TestPropInt2 { get; set; }
    }

    public class TestTrigger : ISMTrigger
    {
        public bool Invoke(ITransition transition, ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            object obj = null;
            if (TransitionParameters.TryGetValue("TargetState", out obj))
                if (obj.ToString() == transition.TargetStateId)
                    return true;

            return false;
        }
    }

    public class TestAction : ISMAction
    {
        [Dependency]
        public StateMachine4Test StateMachine { get; set; }
        public void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            object val;
            if (Parameters.TryGetValue("int", out val))
            {
                StateMachine.Context.TestPropInt += Int32.Parse(val.ToString());
            }
        }
    }

    public class TestActionSetPropTo2 : ISMAction
    {
        private StateMachine4Test StateMachine;
        public TestActionSetPropTo2(StateMachine4Test stateMachine)
        {
            StateMachine = stateMachine;
        }
        
        public void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            StateMachine.Context.TestPropInt2 = 2;
        }
    }

    public class TestInitAction : ISMAction
    {
        [Dependency]
        public StateMachine4Test StateMachine { get; set; }
        public void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            var context = new TestContext();

            object i;
            if (TransitionParameters.TryGetValue("int", out i))
                context.TestPropInt = Int32.Parse(i.ToString());
            object str;
            if (TransitionParameters.TryGetValue("str", out str))
                context.TestPropStr = str.ToString();

            StateMachine.Context = context;
        }
    }


    

    [Serializable]
    public class StateMachine4Test : StateMachineBase<TestContext>, IStateMachine
    {
        #region xml4test
        public static string xml4test = @"
            <StateMachine4Test z:Id=""1"" 
                xmlns:a=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Tests"" 
                xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" 
                xmlns:z=""http://schemas.microsoft.com/2003/10/Serialization/"">
                <Context z:Id=""2"" 
                    xmlns=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"">
                        <Status i:nil=""true""/>
                            <a:_x003C_TestPropInt2_x003E_k__BackingField>2</a:_x003C_TestPropInt2_x003E_k__BackingField>
                            <a:_x003C_TestPropInt_x003E_k__BackingField>15</a:_x003C_TestPropInt_x003E_k__BackingField>
                            <a:_x003C_TestPropStr_x003E_k__BackingField z:Id=""3"">str</a:_x003C_TestPropStr_x003E_k__BackingField>
                </Context>
                <CurrentStateId z:Id=""4"" xmlns=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"">state3</CurrentStateId>
                <Finished xmlns = ""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"">true</Finished >
                <SmId xmlns=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"">e7fc4673-0a9d-4d8e-8c66-1fa1f3eca3d0</SmId>
                <PreviousStateId z:Id=""5"" xmlns=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"">state2</PreviousStateId>
                <StateHistory z:Id=""6"" z:Size=""3"" xmlns=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core"" xmlns:b=""http://schemas.datacontract.org/2004/07/IoC.StateMachine.Core.Classes"">
                    <b:Move z:Id=""7""><b:Order>0</b:Order><b:SourceStateId i:nil=""true""/><b:TargetStateId z:Id=""8"">state1</b:TargetStateId></b:Move>
                    <b:Move z:Id=""9""><b:Order>1</b:Order><b:SourceStateId z:Ref=""8"" i:nil=""true""/><b:TargetStateId z:Ref=""5"" i:nil=""true""/></b:Move>
                    <b:Move z:Id=""10""><b:Order>2</b:Order><b:SourceStateId z:Ref=""5"" i:nil=""true""/><b:TargetStateId z:Ref=""4"" i:nil=""true""/></b:Move>
                </StateHistory>
            </StateMachine4Test> 
        ";
        #endregion
       
    }
}
