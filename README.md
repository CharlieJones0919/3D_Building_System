# 3D_Building_System
 
## Movement Controls:
The camera can be moved using the WASD keys while an object isn't selected. By selecting an object from the hierarchy (list of created object names on the left), the same controls can be used to move said object by also holding down the Left Shift key. All movement is relative to the facing direction of the object.

W = Forward  <br />
A = Left  <br />
S = Back  <br />
D = Right <br />

W + TAB = Up <br />
S + TAB = Down <br />

## Rotation Controls:
The following are the controls for rotation, which are again applicable to the camera and also the selected object with the addition of the Left Shift key. 

E = Y-Axis Clockwise Rotation <br />
Q = Y-Axis Anti-Clockwise Rotation <br />

E + TAB = Z-Axis Clockwise <br />
Q + TAB = Z-Axis Anti-Clockwise Rotation <br />

E + CapsLock = X-Axis Anti-Clockwise Rotation <br />
Q + CapsLock = X-Axis Clockwise Rotation <br />

## Scaling Controls:
These are only applicable to a selected object, but the Left Shift key must still be held to use them.

X + UpArrow = X-Axis Increase Local Scale  <br />
X + DownArrow = X-Axis Decrease Local Scale  <br />

Y + UpArrow = Y-Axis Increase Local Scale  <br />
Y + DownArrow = Y-Axis Decrease Local Scale  <br />
 
Z + UpArrow = Z-Axis Increase Local Scale  <br />
Z + DownArrow = Z-Axis Decrease Local Scale  <br />

## General Implemented UI:
- Create shapes by left-clicking one of the shape icons in the Creation panel.
- Select created shapes by left-clicking their labels in the Hierarchy panel.
- All the controls of shape variables mentioned below can also be implemented by typing new values into the respective field of the Editing panel. This also includes the renaming of the shape.
- Deselect shapes by left-clicking outside the Editing panel (not in the Hierarchy panel).
- Delete shapes by selecting them, then left-clicking the button on the far right of the Creation panel.
- There is a button in the bottom left to reset the camera's position & rotation.
- When the panel is full, the Hierarchy panel's contents can be scrolled through.
