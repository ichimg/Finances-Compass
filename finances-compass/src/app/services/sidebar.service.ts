import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  public leftSideNavToggleSubject: BehaviorSubject<any> = new BehaviorSubject(null);
  public rightSideNavToggleSubject: BehaviorSubject<any> = new BehaviorSubject(null);

  constructor() { } 
 

  public toggleLeftSidebar() {
    return this.leftSideNavToggleSubject.next(null);
  } 

  public toggleRightSidebar() {
    return this.rightSideNavToggleSubject.next(null);
  } 
}