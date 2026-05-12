import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CdbCalculationRequest, CdbCalculationResult } from '../models/cdb.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root'})
export class CdbService {
  private readonly apiUrl =`${environment.apiUrl}/api/cdb/calculate`;

  constructor(private readonly http: HttpClient) {}

  calculate(request: CdbCalculationRequest): Observable<CdbCalculationResult> {
    return this.http.post<CdbCalculationResult>(this.apiUrl, request);
  }
}
