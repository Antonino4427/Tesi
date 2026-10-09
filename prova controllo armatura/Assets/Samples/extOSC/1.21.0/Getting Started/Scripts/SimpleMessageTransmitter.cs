/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

namespace extOSC.Examples
{
	public class SimpleMessageTransmitter : MonoBehaviour
	{
		#region Public Vars  
		// la Region serve per delimitare zone di codice per renderle piuu leggibile e batsa, non influisce il comportamento del codice
		public string Address = "/example/1";

		[Header("OSC Settings")] //Header serve per creare un titolo nella sezione dell'Inspector
        public OSCTransmitter Transmitter; //qui si dichiara una variabile di tipo OSCTransmitter, che è un componente che permette di inviare messaggi OSC
		                                   // la collego al componente transmitter che si trova nell OSCManager

        #endregion

        #region Unity Methods

        protected virtual void Start() 
		{
			var message = new OSCMessage(Address);
			message.AddValue(OSCValue.String("Hello, world!"));

			Transmitter.Send(message);
		}

		#endregion
	}
}