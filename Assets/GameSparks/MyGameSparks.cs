#pragma warning disable 612,618
#pragma warning disable 0114
#pragma warning disable 0108

using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
		public class LogEventRequest_ACCEPT_FRIEND_REQUEST : GSTypedRequest<LogEventRequest_ACCEPT_FRIEND_REQUEST, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_ACCEPT_FRIEND_REQUEST() : base("LogEventRequest"){
			request.AddString("eventKey", "ACCEPT_FRIEND_REQUEST");
		}
		
		public LogEventRequest_ACCEPT_FRIEND_REQUEST Set_request_id( string value )
		{
			request.AddString("request_id", value);
			return this;
		}
		public LogEventRequest_ACCEPT_FRIEND_REQUEST Set_force_add( long value )
		{
			request.AddNumber("force_add", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST : GSTypedRequest<LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "ACCEPT_FRIEND_REQUEST");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST Set_request_id( string value )
		{
			request.AddString("request_id", value);
			return this;
		}
		public LogChallengeEventRequest_ACCEPT_FRIEND_REQUEST Set_force_add( long value )
		{
			request.AddNumber("force_add", value);
			return this;
		}			
	}
	
	public class LogEventRequest_SAVE_PLAYER : GSTypedRequest<LogEventRequest_SAVE_PLAYER, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_SAVE_PLAYER() : base("LogEventRequest"){
			request.AddString("eventKey", "SAVE_PLAYER");
		}
		public LogEventRequest_SAVE_PLAYER Set_XP( long value )
		{
			request.AddNumber("XP", value);
			return this;
		}			
		public LogEventRequest_SAVE_PLAYER Set_GOLD( long value )
		{
			request.AddNumber("GOLD", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_SAVE_PLAYER : GSTypedRequest<LogChallengeEventRequest_SAVE_PLAYER, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_SAVE_PLAYER() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "SAVE_PLAYER");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_SAVE_PLAYER SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_SAVE_PLAYER Set_XP( long value )
		{
			request.AddNumber("XP", value);
			return this;
		}			
		public LogChallengeEventRequest_SAVE_PLAYER Set_GOLD( long value )
		{
			request.AddNumber("GOLD", value);
			return this;
		}			
	}
	
	public class LogEventRequest_XP_INC : GSTypedRequest<LogEventRequest_XP_INC, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_XP_INC() : base("LogEventRequest"){
			request.AddString("eventKey", "XP_INC");
		}
		public LogEventRequest_XP_INC Set_XP( long value )
		{
			request.AddNumber("XP", value);
			return this;
		}			
	}
	
	public class LogChallengeEventRequest_XP_INC : GSTypedRequest<LogChallengeEventRequest_XP_INC, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_XP_INC() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "XP_INC");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_XP_INC SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_XP_INC Set_XP( long value )
		{
			request.AddNumber("XP", value);
			return this;
		}			
	}
	
}
	

namespace GameSparks.Api.Messages {


}
