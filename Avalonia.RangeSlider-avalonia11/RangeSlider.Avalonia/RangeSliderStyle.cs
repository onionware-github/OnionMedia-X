using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using RangeSlider.Avalonia.Enums;

namespace RangeSlider.Avalonia;

public class RangeSliderStyle : AvaloniaObject, IStyle, IResourceProvider
{
	private IStyle _controlsStyles;
	private bool _isLoading;
	private IStyle? _loaded;
	private readonly Uri _baseUri;

	public RangeSliderStyle(Uri baseUri)
	{
		_baseUri = baseUri;
		var uri = new Uri("avares://RangeSlider.Avalonia/Themes/Fluent/RangeSlider.axaml");
		_controlsStyles = new StyleInclude(_baseUri)
		{
			Source = uri,
		};
	}

	public RangeSliderStyle(IServiceProvider serviceProvider)
		: this(((IUriContext)serviceProvider.GetService(typeof(IUriContext))).BaseUri)
	{
	}

	/// <summary>
	/// Get or set the current theme.
	/// </summary>
	public StyleTheme Theme
	{
		set
		{
			var uri = new Uri(value == StyleTheme.Fluent
				? "avares://RangeSlider.Avalonia/Themes/Fluent/RangeSlider.axaml"
				: "avares://RangeSlider.Avalonia/Themes/Material/RangeSlider.axaml");

			_controlsStyles = new StyleInclude(_baseUri)
			{
				Source = uri,
			};
		}
	}

	/// <summary>
	/// Gets the loaded style.
	/// </summary>
	public IStyle Loaded
	{
		get
		{
			if (_loaded != null)
				return _loaded;

			_isLoading = true;

			_loaded = new Styles() { _controlsStyles };

			_isLoading = false;

			return _loaded!;
		}
	}

	public IResourceHost? Owner =>
		(Loaded as IResourceProvider)?.Owner;

	bool IResourceNode.HasResources =>
		(Loaded as IResourceProvider)?.HasResources ?? false;

	public event EventHandler OwnerChanged
	{
		add
		{
			if (Loaded is IResourceProvider rp)
			{
				rp.OwnerChanged += value;
			}
		}
		remove
		{
			if (Loaded is IResourceProvider rp)
			{
				rp.OwnerChanged -= value;
			}
		}
	}

	public bool TryGetResource(object key, out object? value)
	{
		if (!_isLoading && Loaded is IResourceProvider p)
		{
			return p.TryGetResource(key, out value);
		}

		value = null;
		return false;
	}

	void IResourceProvider.AddOwner(IResourceHost owner) =>
		(Loaded as IResourceProvider)?.AddOwner(owner);

	void IResourceProvider.RemoveOwner(IResourceHost owner) =>
		(Loaded as IResourceProvider)?.RemoveOwner(owner);

	SelectorMatchResult IStyle.TryAttach(IStyleable target, object? host)
	{
		return Loaded.TryAttach(target, host);
	}

	IReadOnlyList<IStyle> IStyle.Children =>
		_loaded?.Children ?? Array.Empty<IStyle>();
}