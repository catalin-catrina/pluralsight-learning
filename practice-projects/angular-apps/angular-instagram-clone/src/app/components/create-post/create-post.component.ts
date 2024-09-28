import { AfterContentChecked, Component, inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { ImageUploadService } from '../../services/image-upload.service';
import { AuthenticationService } from '../../services/authentication.service';
import { Auth } from '@angular/fire/auth';

@Component({
  selector: 'app-create-post',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatFormFieldModule, CommonModule],
  templateUrl: './create-post.component.html',
  styleUrl: './create-post.component.scss',
})
export class CreatePostComponent implements AfterContentChecked {
  file: File | null = null;
  description: string = '';
  imageUrl: string | ArrayBuffer | null = null;

  private imageUpload = inject(ImageUploadService);
  private auth = inject(AuthenticationService);
  private firebaseAuth = inject(Auth);

  ngAfterContentChecked(): void {
    console.log('1: ', this.auth.getUser()());
    console.log('2: ', this.firebaseAuth.currentUser);
  }

  onSubmit(form: NgForm) {
    if (this.file) {
      this.imageUpload.uploadFile(this.file);
    }
  }

  onFileSelected(event: Event) {
    const element = event.currentTarget as HTMLInputElement;
    let fileList: FileList | null = element.files;
    if (fileList) {
      this.file = fileList[0];
      this.getImageUrl();
    }
  }

  getImageUrl() {
    const reader = new FileReader();

    reader.onload = (e) => {
      this.imageUrl = e.target?.result || null;
    };

    if (this.file) {
      reader.readAsDataURL(this.file);
    }
  }
}
