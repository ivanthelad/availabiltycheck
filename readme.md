# Track availablity trigger - C<span>#</span>

The `TimerTrigger` makes it incredibly easy to have your functions executed on a schedule. This sample demonstrates a simple use case of calling your function every 5 minutes.

## How it works

The cront trigger executes `_telemetryClient.TrackAvailability(availability)` to track the availablity of a private endpoint and AAD secured endpoint.
### Motivation
  * Privae IP address
  * Secured endpoint. Ideally this code would use MSAL to auth against the backend and refresh its tokens using workload identity 

## Learn more


