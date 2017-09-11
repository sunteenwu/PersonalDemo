using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;

namespace CPathAnimation
{
    public class PathAnimation
    {
        private const int _delay = 30;
        private const double EARTH_RADIUS_KM = 6378.1;

        private DispatcherTimer _timerId;

        private Geopath _path;
        private bool _isGeodesic = false;

        private List<BasicGeoposition> _intervalLocs;
        private List<int> _intervalIdx;

        private int? _duration;
        private int _frameIdx = 0;
        private bool _isPaused;

        public PathAnimation(Geopath path, IntervalCallback intervalCallback, bool isGeodesic, int? duration)
        {
            _path = path;
            _isGeodesic = isGeodesic;
            _duration = duration;

            PreCalculate();
         
            _timerId = new DispatcherTimer();
            _timerId.Interval = new TimeSpan(0, 0, 0, 0, _delay);

            _timerId.Tick += (s, a) =>
            {
                if (!_isPaused)
                {
                    double progress = (double)(_frameIdx * _delay) / (double)_duration.Value;

                    if (progress > 1)
                    {
                        progress = 1;
                    }

                    if (intervalCallback != null)
                    {
                        intervalCallback(_intervalLocs[_frameIdx], _intervalIdx[_frameIdx], _frameIdx);
                    }

                    if (progress == 1)
                    {
                        _timerId.Stop();
                    }

                    _frameIdx++;
                }
            };
        }

        public delegate void IntervalCallback(BasicGeoposition loc, int pathIdx, int frameIdx);

        public Geopath Path
        {
            get { return _path; }
            set
            {
                _path = value;
                PreCalculate();
            }
        }

        public bool IsGeodesic
        {
            get { return _isGeodesic; }
            set
            {
                _isGeodesic = value;
                PreCalculate();
            }
        }

        public int? Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                PreCalculate();
            }
        }

        public void Play()
        {
            _frameIdx = 0;
            _isPaused = false;
            _timerId.Start();
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Stop()
        {
            if (_timerId.IsEnabled)
            {
                _frameIdx = 0;
                _isPaused = false;
                _timerId.Stop();
            }
        }

        private void PreCalculate()
        {
            if (_timerId != null && _timerId.IsEnabled)
            {
                _timerId.Stop();
            }

            _duration = (_duration.HasValue && _duration.Value > 0) ? _duration : 150;

            //_intervalLocs = new Geopath(new List<BasicGeoposition> { });

            _intervalLocs = new List<BasicGeoposition> { };

            _intervalIdx = new List<int>();

            _intervalLocs.Add(_path.Positions[0]);
            _intervalIdx.Add(0);

            double dlat, dlon;
            double totalDistance = 0;

            if (_isGeodesic)
            {
                //Calcualte the total distance along the path in KM's.
                for (var i = 0; i < _path.Positions.Count - 1; i++)
                {
                    totalDistance += HaversineDistance(_path.Positions[i], _path.Positions[i + 1]);
                }
            }
            else
            {
                //Calcualte the total distance along the path in degrees.
                for (var i = 0; i < _path.Positions.Count - 1; i++)
                {
                    dlat = _path.Positions[i + 1].Latitude - _path.Positions[i].Latitude;
                    dlon = _path.Positions[i + 1].Longitude - _path.Positions[i].Longitude;

                    totalDistance += Math.Sqrt(dlat * dlat + dlon * dlon);
                }
            }

            int frameCount = (int)Math.Ceiling((double)_duration.Value / (double)_delay);
            int idx = 0;
            double progress;

            //Pre-calculate step points for smoother rendering.
            for (var f = 0; f < frameCount; f++)
            {
                progress = (double)(f * _delay) / (double)_duration.Value;

                double travel = progress * totalDistance;
                double alpha = 0;
                double dist = 0;
                double dx = travel;

                for (var i = 0; i < _path.Positions.Count - 1; i++)
                {

                    if (_isGeodesic)
                    {
                        dist += HaversineDistance(_path.Positions[i], _path.Positions[i + 1]);
                    }
                    else
                    {
                        dlat = _path.Positions[i + 1].Latitude - _path.Positions[i].Latitude;
                        dlon = _path.Positions[i + 1].Longitude - _path.Positions[i].Longitude;
                        alpha = Math.Atan2(dlat * Math.PI / 180, dlon * Math.PI / 180);
                        dist += Math.Sqrt(dlat * dlat + dlon * dlon);
                    }

                    if (dist >= travel)
                    {
                        idx = i;
                        break;
                    }

                    dx = travel - dist;
                }

                if (dx != 0 && idx < _path.Positions.Count - 1)
                {
                    if (_isGeodesic)
                    {
                        var bearing = CalculateBearing(_path.Positions[idx], _path.Positions[idx + 1]);
                        _intervalLocs.Add(CalculateCoord(_path.Positions[idx], bearing, dx));
                    }
                    else
                    {
                        dlat = dx * Math.Sin(alpha);
                        dlon = dx * Math.Cos(alpha);

                        _intervalLocs.Add(new BasicGeoposition { Latitude = _path.Positions[idx].Latitude + dlat, Longitude = _path.Positions[idx].Longitude + dlon });
                    }

                    _intervalIdx.Add(idx);
                }
            }

            //Ensure the last location is the last coordinate in the path.
            _intervalLocs.Add(_path.Positions[_path.Positions.Count - 1]);
            _intervalIdx.Add(_path.Positions.Count - 1);
        }


        private static double DegToRad(double x)
        {
            return x * Math.PI / 180;
        }

        private static double RadToDeg(double x)
        {
            return x * 180 / Math.PI;
        }

        private static double HaversineDistance(BasicGeoposition origin, BasicGeoposition dest)
        {
            double lat1 = DegToRad(origin.Latitude),
                lon1 = DegToRad(origin.Longitude),
                lat2 = DegToRad(dest.Latitude),
                lon2 = DegToRad(dest.Longitude);

            double dLat = lat2 - lat1,
            dLon = lon2 - lon1,
            cordLength = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2),
            centralAngle = 2 * Math.Atan2(Math.Sqrt(cordLength), Math.Sqrt(1 - cordLength));

            return EARTH_RADIUS_KM * centralAngle;
        }

        private static double CalculateBearing(BasicGeoposition origin, BasicGeoposition dest)
        {
            var lat1 = DegToRad(origin.Latitude);
            var lon1 = origin.Longitude;
            var lat2 = DegToRad(dest.Latitude);
            var lon2 = dest.Longitude;
            var dLon = DegToRad(lon2 - lon1);
            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            return (RadToDeg(Math.Atan2(y, x)) + 360) % 360;
        }

        private static BasicGeoposition CalculateCoord(BasicGeoposition origin, double brng, double arcLength)
        {
            double lat1 = DegToRad(origin.Latitude),
                lon1 = DegToRad(origin.Longitude),
                centralAngle = arcLength / EARTH_RADIUS_KM;

            var lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(centralAngle) + Math.Cos(lat1) * Math.Sin(centralAngle) * Math.Cos(DegToRad(brng)));
            var lon2 = lon1 + Math.Atan2(Math.Sin(DegToRad(brng)) * Math.Sin(centralAngle) * Math.Cos(lat1), Math.Cos(centralAngle) - Math.Sin(lat1) * Math.Sin(lat2));

            return new BasicGeoposition { Latitude = RadToDeg(lat2), Longitude = RadToDeg(lon2) };
        }

    }
}
