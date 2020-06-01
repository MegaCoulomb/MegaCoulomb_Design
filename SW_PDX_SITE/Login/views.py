from django.shortcuts import render, redirect
from django.http import HttpResponse
from datetime import datetime
from django.contrib.auth import login, authenticate
from django.contrib.auth.forms import UserCreationForm
## from django.urls import reverse_lazy
## from django.views import generic

##def index(request):
##    now = datetime.now()

##    return render(
##        request,
##        "Login/PDX_POKER_LAND.html",
##        )

def signup(request):
    if request.method == 'POST':
        form = UserCreationForm(request.POST)
        if form.is_valid():
            form.save()
            username = form.cleaned_data.get('username')
            raw_password = form.cleaned_data.get('password1')
            user = authenticate(username=username, password=raw_password)
            login(request, user)
            return redirect('home')
    else:
        form = UserCreationForm()
    return render(request, 'Login/PDX_POKER_LAND.html', {'form': form})




