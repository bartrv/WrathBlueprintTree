This codebase uses C# .Net MAUI TargetFramework = net8.0-windows10.0.19041.0

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

I'm using BubblePrints/Blueprint Explorer to access the Wrath Blueprint json: https://github.com/factubsio/BubblePrints
  Settings: Export Mode for 'Open in Editor' = 'JBP'
  Bubbleprints produces 99% valid wrath .jbp files, in the 1st entry, "AssedId", the guid has an erronius preceeding $ and there's a missing comma at the end of that line.  These errors have been accounted for in the current version of this code base.
<br>
 ## $\textcolor{red}{Status:\ Pre-alpha,\ NOT\ useable}$
<table>
<tr><td>Read and parse Json:</td><td>yes</td></tr>
<tr><td>Redeploy Json to visual layout:</td><td>partial</td></tr>
<tr><td>Reposition json panels in layout:</td><td>yes</td></tr>
<tr><td>Generate drag list from library:</td><td>yes</td></tr>
<tr><td>Drag/drop new panels:</td><td>yes/partial</td></tr>
<tr><td>Sort/filter list:</td><td>no</td></tr>
<tr><td>Link panel relationships:</td><td>partial/no</td></tr>
<tr><td>Display side panel for data editing:</td><td>partial</td></tr>
<tr><td>Allow data editing:</td><td>no</td></tr>
<tr><td>Allow saving:</td><td>no</td></tr>
<tr><td>Track file open/new/modified/saved purity:</td><td>partial</td></tr>
<tr><td>Undo:</td><td>no</td></tr>
<tr><td>Testing Library:</td><td>yes (Limited)</td></tr>
<tr><td>Modify Library:</td><td>no</td></tr>
<tr><td>Save Library:</td><td>no</td></tr>
</table>
