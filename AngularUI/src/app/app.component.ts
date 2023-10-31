import { Component } from '@angular/core';
import { MedicationsService } from './medications.service';
import { PatientsService } from './patients.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  constructor(private patientsService: PatientsService, private medicationsService: MedicationsService) {
    this.setConnectionStatus(self.navigator.onLine);

    self.addEventListener('online',  this.updateOnlineStatus.bind(this));
    self.addEventListener('offline', this.updateOnlineStatus.bind(this));
  }

  private updateOnlineStatus(): void {
    let isOnline = self.navigator.onLine;
    console.log("Update online status to " + isOnline)
    this.setConnectionStatus(isOnline);
  }

  private setConnectionStatus(isOnline: Boolean): void {
    this.medicationsService.switchUrl(isOnline);
    this.patientsService.switchUrl(isOnline);
  }
}
