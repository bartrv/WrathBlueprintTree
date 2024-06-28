The purpose of this application is to de-mystify Owlcat's Blueprint Modification Template Mod development experience.
In the mods I have created, most of my issues stem from:
1. Consistency - user error while hand coding the json 
2. Obviscation - There is no good documentation.  No list of valid json calls, or which calls are compatible with which objects, or what static values are allowed as options within those objects.
3. Interconnectivity - The Blueprint format isolates most actions and shecks effectively, which means a single ability may have 10 or 20 json files if it's complicated enough
4. Tedium - eg. copying the same file 10x, and manually editing the same 1 line to a different value, new guids, and references, and localization adds for each.  Not hard, but tedious.

To address these issues I'm working tpward the following:
1. Translating the JSON library provided in the Unity Template into a gui structure so the json is generated not typed
2. Create a learnning ingestion system where as new templates are loaded the code library prompts for an explination and incorporates any new options into the referencee library. Eventually, this library may be downloadable/shareable, but it needs to function first.
3. The GUi will use the popular wire-tree interconnect interface that has become the standard for multiple 3D and video effects applications for clearly showing the relationship complicated interactions and dependencies at a glance.
4. Initially this will be a drag/drop interface with copy paste options. Once that is working with all of the above, I'm considering a multi-copy/drop option where tou hold a controlling key while dragging and the conditions of the drop may allow multiple copies to replicate and auto associate

Dev status:
This is currently pre-alpha, and not-useable
