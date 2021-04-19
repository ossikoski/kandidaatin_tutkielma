private async Task HandlePour(int pourAmount)
{
   const string funcName = nameof(HandlePour);
   await _robot.StartPour(pourTimes, startingLocation);
   _PourScale.Tare();
   System.Threading.Thread.Sleep(5000);

   int pourWeight = (pourAmount * 10) - 27; 
   int currentWeight = _PourScale.GetWeight();

   double maxTime = 13;
   DateTime currentTime = DateTime.Now;
   double secondsPassed = 0;

   while (currentWeight < pourWeight && secondsPassed <= maxTime)
   {
      System.Threading.Thread.Sleep(10);
      if (currentWeight < 10 && secondsPassed > 2 
 	  && secondsPassed < 2.1)
      {
         Log.InfoEx(funcName, $"2 seconds passed with no weight: 
                    Scale reports {currentWeight}g. 
	            Attempting to pour again.");
         await _robot.ShakeBottle();
         maxTime += 6;
         System.Threading.Thread.Sleep(3000);
      }
      currentWeight = _PourScale.GetWeight();
      secondsPassed = DateTime.Now.Subtract(currentTime)
         .TotalSeconds;
   }
   if (currentWeight >= pourWeight)
   {
      Log.InfoEx(funcName, 
      $"Pour amount reached with weight of {currentWeight}");
   }
   else
   {
      Log.InfoEx(funcName, 
      $"Pour amount reached by timeout of {maxTime} seconds");
   }
            
   await _robot.StopPour();
}
