# Xenarius Getting Started tutorial
The project in this repository demonstrates how to create a data service for [Xenarius](https://www.xenarius.net) getting started tutorial. For step-by-step instructions on how to use this service to build a simple mobile app, see below. 

### Getting Started Videos:
 To watch the videos that follow the same steps, but based on a ready-to-use data service, go to the [Xenarius.net](https://xenarius.net/docs/videos/#getting-started-part1) website.

### Requirements:

- Visual Studio
- SQL Server 2012 Express LocalDB (to run locally)
- Browser 

## <a name="setup-demoservice"></a>**Setup demo service**
To run the sample locally from Visual Studio:

- Build the solution.
- Open the Package Manager Console (Tools > NuGet Package Manager > Package Manager Console).
- In the Package Manager Console window, enter the following command: `Update-Database`.
- If the previous step has failed, make sure that you have stopped and deleted the database (commands: `sqllocaldb.exe stop v11.0, sqllocaldb.exe delete v11.0 `) and try updating the database once again.
- Press F5 to debug.
- Click the [link](https://localhost:44305/odata/Incidents) for test service

## <a name="register-at-xenatius"></a>**Register at Xenarius.net**
Navigate to [Xenarius Sign In](https://app.xenarius.net/#sign-in) page and click **Create New Account** your enter email. For further login use the passrod that we sending you to your email.

## <a name="setting-up-app"></a>**Application Setup**

To create a new application, click **Create Project**.

To specify the general app properties, click the **Application Options** button on the main menu. This invokes the **Application Options** page enabling you to specify the application name, icon, [global data](https://xenarius.net/docs/bind-widgets-to-data/#global-view-model), [layout](https://xenarius.net/docs/configure-navigation-and-device-specific-layout/#layout) across different platforms, etc.

## <a name="setting-up-data-access"></a>**Data Access Setup**

To enable your Xenarius-powered mobile app to access data, you need to create a [data provider](https://xenarius.net/docs/connect-to-data/#data-connection-basics) that implements a universal interface for reading and modifying data.

Switch back to the project overview page by clicking the application’s top left corner. Click the **Add new data provider...** link on the Data Provider section’s main toolbar and select *Create a new data provider*. On the **Data Provider** settings page, select the **Provider Type** (*OData service*). This provider will connect to a sample OData service and implement the appropriate data access logic.

Next, specify the provider Name (*Incidents*) and a field importing service URL for use in production:

[https://localhost:44305/odata/Incidents](https://localhost:44305/odata/Incidents)

To validate the entered service URL, click Test Query:

![image alt text](https://xenarius.net/images/docs/gs/image_test_query.png)

To learn more about OData providers, see [OData Connection](https://xenarius.net/docs/connect-to-data/#odata-connection).

## <a name="creating-list-view"></a>**Creating a List View**

Let's create the default (startup) application view for listing current incidents. Click the **Views** button on the main toolbar and select *Create a new view*. This opens the created view in the **View Designer**.

To customize the view's settings, use the Properties tab. For example, set the the **Title** property to *Incidents* and add visual components to the view. Drag the List control ![image alt text](https://xenarius.net/images/docs/gs/image_0.png) from the **Editors** gallery and drop it onto the design surface.
To make the list display data from the server, open its **Data Source** property and specify the following values:
 
 1. Change the **Provider or Array** to *Incidents*,
 2. Set the **Sort By** value to the *CreatedOn* field. Also select sorting in descending order.

![image alt text](https://xenarius.net/images/docs/gs/image_default_list_template_with_datasource.png)

## <a name="app-preview"></a> **Application Preview**

To preview the application, click **Run Application** on the Xenarius toolbar. The **Xenarius Simulator** will open on a new browser tab enabling you to test your app across different devices.

You can make a photo of the QR-Code to test the app on an actual mobile device.

![image alt text](https://xenarius.net/images/docs/gs/image_1.png)

Leave the Simulator tab open - you will use it later during the development process. Click **Run Application** to re-launch the app with recent changes made in the designer.

## <a name="list-item-template-customization"></a> **List Item Template Customization**

A list’s item template displays raw data by default, simply transforming JSON to a string. This helps you check the validity of requests to the server and make sure that the list data source setup is correct. To make the list display more meaningful data (for example, the incident’s ID and Subject), select label1 in the Designer or Navigator and bind its Text property to $data.Id.

![image alt text](https://xenarius.net/images/docs/gs/image_2.png)

To bind the *Text* property of the *label1* component to the *Id* data source field, click the property marker (a gray rectangle near the editor), select **Set up binding** and specify *$data.Id* in the editor. Note that data source fields use the *$data* prefix.

Next, add a label for displaying Subject and bind its Text to the $data.Subject field.

![image alt text](https://xenarius.net/images/docs/gs/image_3.png)

To view the result, refresh the app by clicking **Run Application**:

![image alt text](https://xenarius.net/images/docs/gs/image_4.png)

## <a name="navigation-menu"></a> **Adding the View to the Navigation Menu**

The next step is to add the view to the application navigation. Open the application options and expand the **Navigation** section. 

To set up the *Incidents* view as a default application view, select *incidents* view in the **Default View ID** field.

To add this view to the navigation menu as well, do the following:

1. Expand the **Navigation Items** section and click **Add** to create a new item.

2. Specify the item's properties (for example, select the *Incidents* view in the **View ID** list).

3. Set the **Title** property to *Incidents*.

![image alt text](https://xenarius.net/images/docs/gs/image_5.png)

The navigation menu’s location corresponds to the current layout selected in the **Layouts** section. To learn more, see [Configuring navigation and device-specific layouts for an app](https://xenarius.net/docs/configure-navigation-and-device-specific-layout/).

## <a name="creating-the-detail-view"></a> **Creating a Detail View**

Repeat the steps from the previous section to create another view *Incident Details* for browsing and editing incident details. 

All views in Xenarius use caching by default to improve performance. Check the **Refresh When Shown** property to refresh data from the server on opening the *Incident Details* view.

On navigating to this view, a URL parameter should provide information about the incident to be edited. To create a parameter bound to the Incidents OData provider and containing information about the displayed incident, do the following:

1. Switch to the **ViewData & Commands** tab.

2. Click the plus button near **Parameters**.

3. Change the parameter name to *incident*.

4. Select the *Incident* type (indicating that this parameter must load data from the *Incidents* data provider).

![image alt text](https://xenarius.net/images/docs/gs/image_6.png)

It makes sense to use the Form widget for displaying the data at hand. Switch back to the Widget Properties tab and add a **Form** Widget from the **Layouts** group. Customize the form as follows:

1. Add a new **Label** and set its **Title** to the same value as its **ID**. Bind the label’s **Text** to *$model.incident.Id*.

2. Add an **Input** and set its **Title** to the same value as its **Status**. Bind its **Value** to *$model.incident.StatusString*.

3. Add a Textarea and set its **Title** to the same value as its **Subject**. Set its **Value** to *$model.incident.Subject*.

4. Set the **Subject**’s **Style.Size.Height** to *200px*.

The following image illustrates the result:
![image alt text](https://xenarius.net/images/docs/gs/image_7.png)

## <a name="navigating"></a> **Navigating Between Views**

In this tutorial, clicking an item on the *Incidents* view activates the *Incident Details* view and displays details about the selected item. To do this, create an event handler for clicking an item on the *incidentsList*, as follows:

1. Open the *Incidents* view and select the *List1* control.

2. On the **View** editor’s **Properties** tab, click the button in **On Item Click** editor, which opens the code editor.

3. Add the **Navigate to View** operation.

4. Select the *Incident Details* view.

5. Set the **incident** parameter to *$data* (referring to a corresponding **Data Source** list item value).

6. Click **OK** to switch back to the view designer.

![image alt text](https://xenarius.net/images/docs/gs/image_8.png)

Click **Run Application** to view the recent changes in the simulator. Try clicking the Incidents item for testing the navigation to the *Incident Details* view.

## <a name="cud"></a> **Create/Update/Delete**

### <a name="update"></a> **Updating Records**

The *Incident Details* view enables browsing and modifying an incident’s data. For saving the changes made on this view, you need to add a corresponding button.

You can add controls not only to a view's working area, but [add buttons to the application toolbar](https://xenarius.net/docs/adding-commands-to-the-application-toolbar/) as well. To add the **Save** command to the toolbar, do the following:

1. Open the *Incident Details* view in the **View Designer**.

2. Switch to the **View Data & Commands** tab, expand the **Commands** category and click **Add**.

3. Specify the following command properties: **ID**, **Title** and **Icon**.

![image alt text](https://xenarius.net/images/docs/gs/image_9.png)

4. Create a handler for the **OnExecute** event.

In the invoked code editor window, add the **Save** operation from the **Data** tab and specify the parameters as illustrated on the above image. Click **OK** to save the changes.

![image alt text](https://xenarius.net/images/docs/gs/image_10.png)

### <a name="add"></a> **Adding New Records**

The next step is to enable creating new records using the *Incident Details* view. To invoke this view, provide the **Add** command button to the application toolbar, as follows:

1. Switch to the *Incidents* view and create a new command.

2. Create the **OnExecute** event handler for the command and add the **Navigate To View** operation to the function body. Click **Default** and select the *Incident Details* view from the list. The **Add** command opens an empty *Incident* view. Previously, when we were creating the *Incident Details* view, we made the *incident* parameter optional, meaning that it does not need to be specified during navigation. This is why at this step, we can leave the *incident* parameter blank.

To make a new Incident have the *In Progress* status by default, open the **Incident Details** view and switch to the **View Data & Commands** tab. Select **Parameter** *incident* and set the default value for the **StatusString** to *In Progress*, as the following image illustrates.

![image alt text](https://xenarius.net/images/docs/gs/image_11.png)

Click **Refresh application** to restart the application in the simulator and check the process of creating a new Incident.

### <a name="delete"></a> **Deleting Records**

To enable deleting records, activate the corresponding feature of the list control. To do this, open the *Incidents* view and select the list component in the **Properties** tab. In the **Item Operations** section, enable **Allow Deletion** and select the *Slide Item* **Deletion mode**.

## <a name="deploy"></a> **Deploy**

To test your application on a mobile device or publish it to an application store, use the **Deploy** function. Click the **Deploy** button on the main toolbar, and select **Debugging url configuration** or **Production url configuration**. Selecting *production* enables using the Production URL specified in the data provider’s settings. After your browser downloads a zip file with the application you can compile and package it using [PhoneGap Build](http://docs.build.phonegap.com/en_US/#googtrans/(en/)).

![image alt text](https://xenarius.net/images/docs/gs/image_12.png) **[Watch Video](https://youtu.be/Zzd3DqAl4iM?list=PLZdx00eiEOd6t0WKmPlLD21xUULw72xih)**


