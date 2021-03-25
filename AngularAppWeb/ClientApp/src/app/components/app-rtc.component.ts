import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  RtcSignalRService,
  IUser,
  UserConnection,
} from '../services/rtc-signalr.service';

@Component({
  selector: 'app-rtc',
  templateUrl: './app-rtc.component.html',
  styleUrls: ['./app-rtc.component.scss'],
})
export class AppRtcComponent implements OnInit {
  userName = '';
  users: UserConnection[];

  joined = false;

  roomName = 'Test1';

  constructor(
    public rtcService: RtcSignalRService,
    private route: ActivatedRoute
  ) {
    this.route.queryParams.subscribe((data) => {
      if (!data.user) return;
      this.userName = data.user;
    });
    rtcService.usersObservable.subscribe((users) => {
      this.users = users;
    });
  }

  async ngOnInit() {
    await this.rtcService.startConnection();
    this.connect();
  }

  connect() {
    console.log(this.userName);
    this.rtcService.join(this.userName, this.roomName);
    this.joined = true;
  }

  trackByFn(user: IUser) {
    return user.connectionId;
  }
}
