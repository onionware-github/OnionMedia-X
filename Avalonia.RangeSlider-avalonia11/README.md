### About RangeSlider.Avalonia

---

![](./Images/SampleApp.png)

---

For use:

1. Add style in App.xaml

```xml
<Application.Styles>
    ...
    <rangeSlider:RangeSliderStyle/>
</Application.Styles>

```

 For Material.Avalonia add

```xml
<Application.Styles>
    ...
    <rangeSlider:RangeSliderStyle Theme = "Material" />
</Application.Styles>

```

2. Add RangeSlider in your window

```xml
 <Window
    ...
    xmlns:rs="using:RangeSlider.Avalonia.Controls"
    ...>
    
    <Grid>
       <rs:RangeSlider Minimum="0" Maximum="100" LowerSelectedValue="25" UpperSelectedValue="75" IsThumbOverlap="True"/>
    </Grid>
 </Window>
```

---

### License
RangeSlider.Avalonia is licensed under the [MIT license](LICENSE.md).