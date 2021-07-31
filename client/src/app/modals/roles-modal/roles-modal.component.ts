import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
})
export class RolesModalComponent implements OnInit {
  @Input() updateSelectedRoles = new EventEmitter();
  @Input() internalData;
  
  roles: string[] = [];
  user: User;

  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {
    this.user = this.internalData.user;
    this.roles = this.internalData.roles;
  }

  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.activeModal.close();
  }
}
