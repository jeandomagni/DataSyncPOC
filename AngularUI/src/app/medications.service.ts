import { Injectable } from '@angular/core';
import { HttpClient } from  '@angular/common/http';
import { Observable, of } from 'rxjs';

import { MedicationModel } from './medication.model';
import { BaseService } from './base.service';

@Injectable({ providedIn: 'root' })
export class MedicationsService extends BaseService {
  protected offlineUrl = 'https://localhost:7151/medications';
  protected onlineUrl = 'https://remotemedicationsapi.azurewebsites.net/medications';
  private url = this.offlineUrl

  constructor(private http: HttpClient) { 
    super();
  }

  switchUrl(online: Boolean) {
    if(online) {
      this.url = this.onlineUrl
    } else {
      this.url = this.offlineUrl
    }
  }

  getMedications(): Observable<MedicationModel[]> {
    console.log("Get Medications called with url: " + this.getUrl())
    return this.http.get<MedicationModel[]>(this.getUrl());
  }

  saveMedication(medication: MedicationModel): Observable<MedicationModel> {
    console.log("Save Medication called with url: " + this.getUrl())
    return this.http.post<MedicationModel>(this.getUrl(), medication);
  }

  removeMedication(medication: MedicationModel): Observable<Boolean> {
    console.log("Remove Medication called with url: " + this.getUrl())
    return this.http.delete<Boolean>(this.getUrl(), { body: medication });
  }
}