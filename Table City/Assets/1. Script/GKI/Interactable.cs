using Oculus.Interaction.Surfaces;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : PointerInteractable<PokeInteractor, PokeInteractable>
{
    [SerializeField, Interface(typeof(ISurfacePatch))]
    private UnityEngine.Object _surfacePatch;
    public ISurfacePatch SurfacePatch { get; private set; }

    private float _enterHoverNormal = 0.03f;
    private float _enterHoverTangent = 0;
    private float _exitHoverNormal = 0.05f;
    private float _exitHoverTangent = 0f;
    private float _cancelSelectNormal = 0.3f;
    private float _cancelSelectTangent = 0.03f;

    public class MinThresholdsConfig
    {
        public bool Enabled;
        public float MinNormal = 0.01f;
    }

    private MinThresholdsConfig _minThresholds =
        new MinThresholdsConfig()
        {
            Enabled = false,
            MinNormal = 0.01f
        };

    public class DragThresholdsConfig
    {
        public bool Enabled;
        public float DragNormal;
        public float DragTangent;
        public ProgressCurve DragEaseCurve;
    }

    private DragThresholdsConfig _dragThresholds =
        new DragThresholdsConfig()
        {
            Enabled = true,
            DragNormal = 0.01f,
            DragTangent = 0.01f,
            DragEaseCurve = new ProgressCurve(AnimationCurve.EaseInOut(0, 0, 1, 1), 0.05f)
        };

    public class PositionPinningConfig
    {
        public bool Enabled;
        public float MaxPinDistance;
    }

    private PositionPinningConfig _positionPinning =
        new PositionPinningConfig()
        {
            Enabled = false,
            MaxPinDistance = 0f
        };

    public class RecoilAssistConfig
    {
        public bool Enabled;
        public float ExitDistance;
        public float ReEnterDistance;
    }

    private RecoilAssistConfig _recoilAssist =
        new RecoilAssistConfig()
        {
            Enabled = false,
            ExitDistance = 0.02f,
            ReEnterDistance = 0.02f
        };

    private float _closeDistanceThreshold = 0.001f;

    private int _tiebreakerScore = 0;

    #region Properties

    public float EnterHoverNormal
    {
        get
        {
            return _enterHoverNormal;
        }

        set
        {
            _enterHoverNormal = value;
        }
    }

    public float EnterHoverTangent
    {
        get
        {
            return _enterHoverTangent;
        }

        set
        {
            _enterHoverTangent = value;
        }
    }

    public float ExitHoverNormal
    {
        get
        {
            return _exitHoverNormal;
        }

        set
        {
            _exitHoverNormal = value;
        }
    }

    public float ExitHoverTangent
    {
        get
        {
            return _exitHoverTangent;
        }

        set
        {
            _exitHoverTangent = value;
        }
    }

    public float CancelSelectNormal
    {
        get
        {
            return _cancelSelectNormal;
        }

        set
        {
            _cancelSelectNormal = value;
        }
    }

    public float CancelSelectTangent
    {
        get
        {
            return _cancelSelectTangent;
        }

        set
        {
            _cancelSelectTangent = value;
        }
    }

    public float CloseDistanceThreshold
    {
        get
        {
            return _closeDistanceThreshold;
        }
        set
        {
            _closeDistanceThreshold = value;
        }
    }

    public int TiebreakerScore
    {
        get
        {
            return _tiebreakerScore;
        }
        set
        {
            _tiebreakerScore = value;
        }
    }

    public MinThresholdsConfig MinThresholds
    {
        get
        {
            return _minThresholds;
        }

        set
        {
            _minThresholds = value;
        }
    }

    public DragThresholdsConfig DragThresholds
    {
        get
        {
            return _dragThresholds;
        }

        set
        {
            _dragThresholds = value;
        }
    }

    public PositionPinningConfig PositionPinning
    {
        get
        {
            return _positionPinning;
        }

        set
        {
            _positionPinning = value;
        }
    }

    public RecoilAssistConfig RecoilAssist
    {
        get
        {
            return _recoilAssist;
        }

        set
        {
            _recoilAssist = value;
        }
    }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        SurfacePatch = _surfacePatch as ISurfacePatch;
    }

    protected override void Start()
    {
        this.BeginStart(ref _started, () => base.Start());
        this.AssertField(SurfacePatch, nameof(SurfacePatch));

        _exitHoverNormal =
            Mathf.Max(_enterHoverNormal, _exitHoverNormal);

        _exitHoverTangent =
            Mathf.Max(_enterHoverTangent, _exitHoverTangent);

        if (_cancelSelectTangent > 0)
        {
            _cancelSelectTangent =
                Mathf.Max(_exitHoverTangent, _cancelSelectTangent);
        }

        if (_minThresholds.Enabled && _minThresholds.MinNormal > 0f)
        {
            _minThresholds.MinNormal =
                Mathf.Min(_minThresholds.MinNormal,
                _enterHoverNormal);
        }
        this.EndStart(ref _started);
    }

    public bool ClosestSurfacePatchHit(Vector3 point, out SurfaceHit hit)
    {
        return SurfacePatch.ClosestSurfacePoint(point, out hit);
    }

    public bool ClosestBackingSurfaceHit(Vector3 point, out SurfaceHit hit)
    {
        return SurfacePatch.BackingSurface.ClosestSurfacePoint(point, out hit);
    }

    #region Inject

    public void InjectAllPokeInteractable(ISurfacePatch surfacePatch)
    {
        InjectSurfacePatch(surfacePatch);
    }

    public void InjectSurfacePatch(ISurfacePatch surfacePatch)
    {
        _surfacePatch = surfacePatch as UnityEngine.Object;
        SurfacePatch = surfacePatch;
    }

    #endregion
}
