# ConsoleControlLibrary

GDI+ implemented input and output controls.

Contains button, textbox, checkbox, radio button, label, textblock and picture.

Controls are added to forms (inherits the `ConsoleForm` class) and a form is added to the `ConsoleControl` WinForms control.

Example: [https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/Form1.cs](https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/Form1.cs)

## Picture

Usage: Add a `ClientPicture` to a form and send the instance to an object inherited from `VectorImageBase`. Override the `DrawPicture` method.

Example: [https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/TextAdventureForm.cs](https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/TextAdventureForm.cs)

Alternatively, add a `ClientPicture` to a form and send the instance to a `TextBasedVectorImage` together with a string that describes the image.

Example: [https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/TextBasedVectorImageForm.cs](https://github.com/Anders-H/ConsoleControlLibrary/blob/master/ControlsTestProject/TextBasedVectorImageForm.cs)
