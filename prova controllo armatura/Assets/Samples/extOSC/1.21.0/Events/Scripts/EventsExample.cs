/* Copyright (c) 2024 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEngine.UI;

namespace extOSC.Examples
{
	public class EventsExample : MonoBehaviour
	{
		#region Public Vars

		public OSCTransmitter Transmitter;

		[Header("UI Settings")]
		public Text TextRotate;

		public Text TextScale;

		public Text TextPosition;

		#endregion

		#region Private Vars

		private const string _rotateAddress = "/example/3/rotate";

		private const string _scaleAddress = "/example/3/scale";

		private const string _positionAddress = "/example/3/position";

		private Vector3 _position = Vector3.zero;

		#endregion

		#region Unity Methods

		protected virtual void Start() //protected significa che questo metodo può essere chiamato solo da questa classe o da classi derivate,
									   //virtual significa che può essere sovrascritto in classi derivate
		{
			TextScale.text = $"{Vector3.one}";
			TextRotate.text = $"{Vector3.zero}";
			TextPosition.text = $"{Vector3.zero}";
		}

		#endregion

		#region Public Methods  
		//i metodi pubblici venogno chiamati dall'interfaccia utente quando si muovono gli slider 

		public void SendRotate(float value)
		{
			value = OSCUtilities.Map(value, 0, 1, 0, 360); //oscutilities.map serve per mappare un valore da un intervallo a un altro, in questo caso da 0-1 a 0-360

            var vector = new Vector3(0, 0, value);

			SendVector(_rotateAddress, vector);

			TextRotate.text = vector.ToString();
		}

		public void SendScale(float value)
		{
			value = OSCUtilities.Map(value, 0, 1, 1, 5);

			var vector = new Vector3(value, value, value);

			SendVector(_scaleAddress, vector);

			TextScale.text = vector.ToString();
		}

		public void SendPosition(Vector2 value)
		{
			_position.x = OSCUtilities.Map(value.x, -1, 1, -100, 100);
			_position.y = OSCUtilities.Map(value.y, -1, 1, -100, 100);

			SendVector(_positionAddress, _position);

			TextPosition.text = _position.ToString();
		}

		#endregion

		#region Private Methods

		private void SendVector(string address, Vector3 vector) //questa funzione manda il vettore al manager che la gira in base all address,
																//che sono collegati a dei component nell UI Panel->Background Image-> event Item
																//che chiama le funzioni pubbliche quando riceve qualcosa dal OSCmanager
		{
			var message = new OSCMessage(address); 

			message.AddValue(OSCValue.Float(vector.x)); 
			message.AddValue(OSCValue.Float(vector.y));
			message.AddValue(OSCValue.Float(vector.z));

			if (Transmitter != null)
				Transmitter.Send(message);
		}

		#endregion
	}
}