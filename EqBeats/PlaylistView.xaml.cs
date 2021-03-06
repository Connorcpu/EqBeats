﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EqBeatsAPI;
using EqBeatsPlaybackAgent;
using Microsoft.Phone.BackgroundAudio;

namespace EqBeats {
    public partial class PlaylistView {
        public PlaylistView() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            var id = int.Parse(NavigationContext.QueryString["id"]);
            ResourceManager.GetPlaylistById(
                id, playlist => Dispatcher.BeginInvoke(() => {
                    DataContext = playlist;
                }),
                exception => Dispatcher.BeginInvoke(() => MessageBox.Show("")));
        }

        private void PlayButtonClicked(object sender, RoutedEventArgs e) {
            var button = (Button)sender;
            var song = (Track)button.DataContext;
            var playlist = (Track[])GetParentControl(button).ItemsSource;
            var index = Array.IndexOf(playlist, song);

            AudioPlayer.SavePlaylist(playlist, index);

            BackgroundAudioPlayer.Instance.Track = song.ToAudioTrack("Update");
            BackgroundAudioPlayer.Instance.Play();
            NavigationService.Navigate(new Uri("/NowPlaying.xaml", UriKind.Relative));
        }

        private static ItemsControl GetParentControl(DependencyObject button) {
            var parent = button;
            while (!(parent is ItemsControl)) {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (ItemsControl)parent;
        }
    }
}