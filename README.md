The main feature of this test was to control enemy visibility depending on where the player is. To handle player-enemy communication, I created an event manager based on Ids for better readability. The event manager can be expandable even though for now it only manages one event. Every time an entity changes zones, an event is published and received across all enemies to update its visibility.

For enemy creation (using letter G) I implemented a pool of enemies to avoid extra memory allocation. Enemies can only be created if the player is inside a zone and are created in the last position entered.

To access these services I used a service locator to provide a single global point.

Every time a zone is created in the hierarchy, a unique Id is assigned. It is then added to a Scriptable Object that stores the zone id and the name. This way, using an Editor script, we can assign to each zone the neighbours with a dropdown showing all the names of the zones retrieved from the Scriptable Object. The names can be modified at any time changing the name of the Game Object itself.

There are two zone templates in the prefab folder with different colliders that can be used but in case a new zone is needed, it has to have the layer ‘Zone’ in the parent, as well as a trigger collider, a Zone.cs and ZoneIdConfig.cs (added in that order). 

