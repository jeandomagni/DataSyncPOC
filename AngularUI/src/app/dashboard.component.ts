import { Component, OnInit } from '@angular/core';
import { PatientModel } from './patient.model';
import { MedicationModel } from './medication.model';
import { PatientsService } from './patients.service';
import { MedicationsService } from './medications.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  patients: PatientModel[] = [];
  medications: MedicationModel[] = [];

  newPatient: PatientModel = new PatientModel();
  newMedication: MedicationModel = new MedicationModel();

  constructor(private patientsService: PatientsService, private medicationsService: MedicationsService) { }

  ngOnInit(): void {
    this.getPatients();
    this.getMedications();
  }

  getPatients(): void {
    this.patientsService.getPatients()
      .subscribe((patients: PatientModel[]) => this.patients = patients);
  }
  getMedications(): void {
    this.medicationsService.getMedications()
      .subscribe((medications: MedicationModel[]) => this.medications = medications);
  }

  addNewMedication(): void {
    this.medicationsService.saveMedication(this.newMedication).subscribe((createdMedication: MedicationModel) => {
      this.medications.push(createdMedication);
      this.newMedication = new MedicationModel();
    });
  }

  addNewPatient(): void {
    this.patientsService.saveNewPatient(this.newPatient).subscribe((createdPatient: PatientModel) => {
    this.patients.push(createdPatient);
      this.newPatient = new PatientModel();
    });
    
  }

  removeMedication(m: MedicationModel): void {
    this.medicationsService.removeMedication(m).subscribe(() => {
      this.removeItemOnce(this.medications, m);
    });
  }

  removePatient(p: PatientModel): void {
    this.patientsService.removePatient(p).subscribe(() => {
      this.removeItemOnce(this.patients, p);
    });
  }

  removeItemOnce(arr: any[], value: any) {
    var index = arr.indexOf(value);
    if (index > -1) {
      arr.splice(index, 1);
    }
    return arr;
  }
}