import { Injectable } from '@angular/core';
import { HttpClient } from  '@angular/common/http';
import { Observable, of } from 'rxjs';

import { PatientModel } from './patient.model';
import { BaseService } from './base.service';

@Injectable({ providedIn: 'root' })
export class PatientsService extends BaseService {
  protected offlineUrl = 'https://localhost:7206/patients';
  protected onlineUrl = 'https://remotepatientsapi.azurewebsites.net/patients';
  private url = this.offlineUrl


  constructor(private http: HttpClient) { 
    super()
  }

  switchUrl(online: Boolean) {
    if(online) {
      this.url = this.onlineUrl
    } else {
      this.url = this.offlineUrl
    }
  }

  getPatients(): Observable<PatientModel[]> {
    console.log("Get Patients called with url: " + this.getUrl())
    return this.http.get<PatientModel[]>(this.getUrl());
  }

  saveNewPatient(patient: PatientModel): Observable<PatientModel> {
    console.log("Save Patient called with url: " + this.getUrl())
    return this.http.post<PatientModel>(this.getUrl(), patient);
  }

  removePatient(patient: PatientModel): Observable<Boolean> {
    console.log("Remove Patient called with url: " + this.getUrl())
    return this.http.delete<Boolean>(this.getUrl(), { body: patient });
  }
}